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
    /// Represents a single team in the server
    ///////////////////////////////////////////////////////
    public partial class Team : CustomObject
    {	// Member variables
        ///////////////////////////////////////////////////
        private GameClient _game;
        private Arena _arena;
        public string _name;
        public int _id;
        public Dictionary<ushort, Player> _players;		//The players in our team, indexed by id
        public int _playerCount = 0;


        public Team(Arena arena, GameClient game)
        {
            _game = game;
            _arena = arena;
            _players = new Dictionary<ushort, Player>();
        }

        #region Player Events
        /// <summary>
        /// Fired when a player leaves his team
        /// </summary>
        /// <param name="playerID"></param>
        public void playerLeave(ushort playerID)
        {
            //bye!
            _players.Remove(playerID);
        }

        /// <summary>
        /// Fired when a player joins a team
        /// </summary>
        /// <param name="player"></param>
        public void playerJoin(Player player)
        {
            //Leave his old team if he has one.
            if (player._team != null)
                player._team.playerLeave(player._id);

            player._team = this;

            //Update our dictionary
            _players.Add(player._id, player);
        }

        #endregion

        #region Player Accessors
        /// <summary>
        /// Gets a player of the specified id
        /// </summary>
        public Player getPlayerById(uint id)
        {	//Attempt to find him
            foreach (Player player in _players.Values)
                if (player._id == id)
                    return player;

            return null;
        }
        #endregion



    }
}