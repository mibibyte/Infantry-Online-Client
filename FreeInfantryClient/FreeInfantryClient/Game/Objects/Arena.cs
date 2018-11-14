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


namespace FreeInfantryClient.Game
{
    // Player Class
    /// Represents a single player in the server
    ///////////////////////////////////////////////////////
    public partial class Arena : CustomObject
    {	// Member variables
        ///////////////////////////////////////////////////
        private GameClient _game;
        public string _name;
        public int _id;
        private Dictionary<ushort, Player> _players;		//The players in our arena, indexed by id


        public Arena(GameClient game)
        {
            _game = game;
            _players = new Dictionary<ushort, Player>();
        }

        public void playerLeave(ushort id)
        {
            //bye!
            _players.Remove(id);

            //Update our GUI
            _game._wGame.updatePlayerList(_players);
        }

        public void playerEnter(SC_PlayerEnter pkt)
        {
            //Create a new player instance
            Player player = new Player();
            player._alias = pkt.singlePlayer._alias;
            player._id = pkt.singlePlayer._id;

            //Update our dictionary
            _players.Add(player._id, player);

            //Update our GUI
            _game._wGame.updatePlayerList(_players);
        }
    }
}