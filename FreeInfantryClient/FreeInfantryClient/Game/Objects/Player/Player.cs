using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

using InfServer.Network;
using InfServer.Protocol;
using InfServer.Logic;

using Assets;
using FreeInfantryClient.Settings;


namespace FreeInfantryClient.Game
{
    // Player Class
    /// Represents a single player in the server
    ///////////////////////////////////////////////////////
    public partial class Player : CustomObject
    {	// Member variables
        ///////////////////////////////////////////////////
        public Client _client;					//Our network client

        public volatile bool bDestroyed;		//Have we already been destroyed?
        public bool _bIngame;					//Are we in the game, or in an arena transition?
        public bool _bLoggedIn;					//Have we made it past the login process, and are able to enter arenas?
        public List<DateTime> _msgTimeStamps;   //For spam checking
        public GameClient _gameclient;
        public string _lastPM = null;
        public List<string> _chats;             //The chats we belong to
        public Team _team;                      //The team we belong to

        public Player(bool local)
        {
            _state = new Helpers.ObjectState();
        }

        #region Credentials
        public ushort _id;						//Unique zone id for a player
        public int _magic;						//Magic id used for distinguishing players with similiar id
        public string _ticketid;

        public string _alias;					//Our current name
        public string _squad;					//The squad he belongs to
        public long _squadID;

        public IPAddress _ipAddress;
        public uint _UID1;
        public uint _UID2;
        public uint _UID3;
        #endregion

        #region Game state
        public bool _bIgnoreUpdates;			//Are we temporarily ignoring player updates? (Usually due to vehicle change)
        public bool _bSpectator;				//Is the player in spectator mode?
        public bool _bIsStealth;                //Is the mod hidden to player lists?
        public int _level;                      //The players level

        public Helpers.ObjectState _state;		//The player's positional state

        public bool _bEnemyDeath;				//Was the player killed by an enemy, or teammate?
        public int _deathTime;					//The tickcount at which we were killed

        public int _lastItemUseID;				//The id and ticktime at which the last item
        public int _lastItemUse;				//was fired.

        public int _lastWarpItemUseID;          //The id and ticktime at which the last warp items
        public long _lastWarpItemUse;           //was fired.

        public int _lastVehicleEntry;           //The tick at which the player last entered or exited a vehicle

        public int _lastMovement;               //The tickcount at which the player last made a movement
        public uint _assetCS;

        #endregion

        #region Social
        public void loadChats()
        {

            string chats = "";
            foreach (string chat in GameSettings.Chats._chats)
            {
                chats += chat + ",";
            }

            chats = chats.TrimEnd(',');

            _gameclient.sendChat(string.Format("?chat {0}", chats), "", InfServer.Protocol.Helpers.Chat_Type.Normal);


        }
        #endregion 

    }
}