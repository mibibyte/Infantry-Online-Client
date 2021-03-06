﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Threading;
using System.Text;
using System.IO;
using FreeInfantryClient.Windows;
using InfServer.Network;
using InfServer.Protocol;
using InfServer;
using FreeInfantryClient.Game.Commands;
using FreeInfantryClient.Game.Protocol;
using Assets;


namespace FreeInfantryClient.Game
{
    public partial class GameClient : IClient
    {
        public InfServer.LogClient _logger;      //Our log client for database related activities
        public Windows.Game _wGame;              //Our game form
        public Player _player;                   //Our player instance
        public Arena _arena;                     //The arena we currently belong to
        public ClientConn<GameClient> _conn;     //Our UDP connection client

        public ManualResetEvent _syncStart;     //Used for blocking connect attempts
        public bool _bLoginSuccess;             //Were we able to successfully login?
        public int _connectionID;
        public bool _bInGame;
        public Commands.Registrar _commandRegistrar;    //Our chat command registrar

        public CfgInfo _zoneConfig;				//The zone-specific configuration file

        public GameClient(Windows.Game wGame, string alias, string ticketid)
        {
            _wGame = wGame;
            _conn = new ClientConn<GameClient>(new S2CPacketFactory<GameClient>(), this);
            _syncStart = new ManualResetEvent(false);

            _player = new Player(true);
            _player._gameclient = this;

            _player._ticketid = ticketid;
            _player._alias = alias;

            //Log packets for now..
            _conn._bLogPackets = true;

            _logger = InfServer.Log.createClient("Client");
            _conn._logger = _logger;
            Client.connectionTimeout = 2000;

            //Initialize our command registrar
            _commandRegistrar = new FreeInfantryClient.Game.Commands.Registrar();
            _commandRegistrar.register();
        }

        public void quit()
        {
            //Clean up stuff
            gameClosing();
           
            //Send our disconnect signal
            disconnect();

            //Close our game form.
            _wGame.FadeOut(80);

        }



        #region Connection
        /// <summary>
        /// Called when making a connection to a zoneserver
        /// </summary>
        public void connect(IPEndPoint sPoint)
        {
            _syncStart.Set();

            InfServer.Log.write("Connecting to {0}", sPoint);
            _wGame.updateChat(String.Format("Connecting to {0}", sPoint), "Client", InfServer.Protocol.Helpers.Chat_Type.System, "");

            //Start our connection
            _conn.begin(sPoint);

            //Send our initial packet
            CS_Initial init = new CS_Initial();
            int connectionID = new Random().Next();
            init.connectionID = connectionID;
            _conn._client._connectionID = connectionID;
            _connectionID = connectionID;

            init.CRCLength = 0;
            init.udpMaxPacket = 496;


            //Send our init!
            _conn._client.send(init);

        }

        /// <summary>
        /// Looks after the gamestate
        /// </summary>
        public void handleState()
        {
            new Thread(() =>
            {



                //Get an image of the arena list
                int lastArenaUpdate = Environment.TickCount;

                while (_bInGame)
                {   //Is it time to update our list yet?
                    if (Environment.TickCount - lastArenaUpdate > 1000)
                    {
                        lastArenaUpdate = Environment.TickCount;


                        try
                        {
                            using (LogAssume.Assume(_logger))
                                _arena.poll();
                        }
                        catch (Exception ex)
                        {
                            Log.write(TLog.Exception, "Exception whilst polling arena {0}:\r\n{1}", _arena._name, ex);
                        }

                        try
                        {
                            using (LogAssume.Assume(_logger))
                                _player.poll();
                        }
                        catch (Exception ex)
                        {
                            Log.write(TLog.Exception, "Exception whilst polling playerstate {0}:\r\n{1}", _arena._name, ex);
                        }
                    }

                    // Sleep for a bit
                    Thread.Sleep(5);

                }
            }).Start();
        }


        /// <summary>
        /// Sends a reliable packet to the client
        /// </summary>
        public void send(PacketBase packet)
        {	//Relay
            send(packet, null);
        }

        /// <summary>
        /// Sends a reliable packet to the client
        /// </summary>
        public void send(PacketBase packet, Action completionCallback)
        {	//Defer to our client!
            _conn._client.sendReliable(packet, completionCallback);
        }

        /// <summary>
        /// Disconnects our current session with the server
        /// </summary>
        public void disconnect()
        {
            CS_Disconnect discon = new CS_Disconnect();
            discon.connectionID = _conn._client._connectionID;
            discon.reason = CS_Disconnect.DisconnectReason.DisconnectReasonApplication;
            _conn._client.send(discon);
            _conn._client.destroy();
        }

        /// <summary>
        /// Allows our client object to be destroyed properly
        /// </summary>
        public void destroy()
        {
            _conn._logger.close();
            _conn._client._bDestroyed = true;
        }

        /// <summary>
        /// Retrieves the client connection stats
        /// </summary>
        public Client.ConnectionStats getStats()
        {
            return _conn._client._stats;
        }

        /// <summary>
        /// Retrieves our connection status to the database server
        /// </summary>
        public bool IsConnected
        {
            get
            {
                return _conn.IsConnected;
            }
        }
        #endregion

        public void gameLoaded()
        {
            _bInGame = true;
            handleState();
            _player.loadChats();
        }

        public void gameClosing()
        {
            _bInGame = false;
        }

        #region Social Updates
        public void sendChat(string message, string recipient, InfServer.Protocol.Helpers.Chat_Type type)
        {
            CS_Chat chat = new CS_Chat();
            chat.chatType = type;
            chat.bong = 0;
            chat.message = message;
            chat.recipient = recipient;

            _conn._client.send(chat);

            _wGame.updateChat(message, _player._alias, type, "");
        }
        #endregion

        #region Arena Handling
        public void joinArena(string arena)
        {

            _arena = new Arena(this);
            _arena._name = arena;

            CS_ArenaJoin join = new CS_ArenaJoin();
            join.EXEChecksum = 1;
            join.Unk1 = false;
            join.AssetChecksum = 3;
            join.ArenaName = arena;
            _conn._client.send(join);

        }
        #endregion

        #region Command Handling
        /// <summary>
        /// Handles a typical chat command received from a player
        /// </summary>
        public void playerChatCommand(Player from, Player recipient, string command, string payload, int bong)
        {	//Attempt to find the appropriate handler
            HandlerDescriptor handler;

            if (!_commandRegistrar._chatCommands.TryGetValue(command.ToLower(), out handler))
            {
                InfServer.Log.write("Attempt to use unknown command: [{0}]", command);
                return;
            }

            try
            {	//Handle it!
                handler.handler(from, recipient, payload, bong);
            }
            catch (Exception ex)
            {
                if (recipient != null)
                    InfServer.Log.write(InfServer.TLog.Exception, "Exception while executing chat command '{0}' from '{1}' to '{2}'.\r\nPayload: {3}\r\n{4}",
                        command, from, recipient, payload, ex);
                else
                    InfServer.Log.write(InfServer.TLog.Exception, "Exception while executing chat command '{0}' from '{1}'.\r\nPayload: {2}\r\n{3}",
                        command, from, payload, ex);
            }
        }
        #endregion
    }
}
