using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Threading;
using FreeInfantryClient.Windows;
using InfServer.Network;
using InfServer.Protocol;
using FreeInfantryClient.Game.Protocol;

namespace FreeInfantryClient.Game.Logic
{
    public partial class Login
    {
        /// <summary>
        /// Handles the initial packet sent by the server
        /// </summary>
        static public void Handle_SC_Initial(SC_Initial pkt, Client client)
        {
            CS_State csi = new CS_State();


            csi.tickCount = (ushort)Environment.TickCount;
            csi.packetsSent = client._packetsSent;
            csi.packetsReceived = client._packetsReceived;

            client.send(csi);
        }

        /// <summary>
        /// Handles the servers's state packet
        /// </summary>
        static public void Handle_SC_State(SC_State pkt, Client client)
        {	//Consider the connection started now, time to send our login info..

            GameClient c = ((client as Client<GameClient>)._obj);
            c._syncStart.Set();

            CS_Login login = new CS_Login();

            //Lets put some bogus stuff together...
            login.bCreateAlias = false;
            login.UID1 = 99999;
            login.UID2 = 99999;
            login.UID3 = 99999;
            login.NICInfo = 4;
            login.SysopPass = "";
            login.Username = c._player._alias;
            login.Version = (ushort)154;
            login.TicketID = c._player._ticketid;

            //Send it!
            client.send(login);
        }


        /// <summary>
        /// Handles the server's login reply
        /// </summary>
        static public void Handle_SC_Login(SC_Login pkt, Client client)
        {
            GameClient c = ((client as Client<GameClient>)._obj);
            c._syncStart.Set();

            InfServer.Log.write(String.Format("(Result={0}) -  (Config={1}) - (Message={2})", pkt.result, pkt.zoneConfig, pkt.popupMessage));
            c._wGame.updateChat(String.Format("(Result={0}) -  (Config={1}) - (Message={2})", 
                pkt.result, pkt.zoneConfig, pkt.popupMessage), "ZoneServer", InfServer.Protocol.Helpers.Chat_Type.PrivateChat,"");

            //No sense in being connected anymore
            if (pkt.result == SC_Login.Login_Result.Failed)
            {
                Disconnect discon = new Disconnect();
                discon.connectionID = client._connectionID;
                discon.reason = Disconnect.DisconnectReason.DisconnectReasonApplication;
                client.send(discon);
                return;
            }

            //Must have been a success, lets let the server know we're ready.
            client.send(new CS_Ready());
        }

        /// <summary>
        /// Handles the server's PatchInfo reply
        /// </summary>
        static public void Handle_SC_PatchInfo(SC_PatchInfo pkt, Client client)
        {
           InfServer.Log.write("(PatchServer={0}:{1}) (Xml={2})", pkt.patchServer, pkt.patchPort, pkt.patchXml);
        }

        /// <summary>
        /// Handles the server's AssetInfo reply
        /// </summary>
        static public void Handle_SC_AssetInfo(SC_AssetInfo pkt, Client client)
        {
            //We're past the login process, let's join an arena!
            GameClient c = ((client as Client<GameClient>)._obj);
            //Blank arena name = first available public arena
            c.joinArena("");
        }

        /// <summary>
        /// Handles the server's SetIngame reply
        /// </summary>
        static public void Handle_SC_SetIngame(SC_SetIngame pkt, Client client)
        {
            GameClient c = ((client as Client<GameClient>)._obj);
            c._syncStart.Set();

            c._wGame.updateChat("We're in!", "ZoneServer", InfServer.Protocol.Helpers.Chat_Type.System, "");

        }
        /// <summary>
        /// Registers all handlers
        /// </summary>
        [InfServer.Logic.RegistryFunc]
        static public void Register()
        {
            SC_Initial.Handlers += Handle_SC_Initial;
            SC_State.Handlers += Handle_SC_State;
            SC_Login.Handlers += Handle_SC_Login;
            SC_PatchInfo.Handlers += Handle_SC_PatchInfo;
            SC_AssetInfo.Handlers += Handle_SC_AssetInfo;
            SC_SetIngame.Handlers += Handle_SC_SetIngame;
        }
    }
}
