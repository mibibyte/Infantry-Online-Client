using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Threading;
using System.Text;
using System.IO;
using FreeInfantryClient.Windows;
using FreeInfantryClient.Game.Commands;
using FreeInfantryClient.Game.Protocol;

using InfServer;
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
        public Dictionary<ushort, Player> _players;		//The players in our arena, indexed by id
        protected Dictionary<string, Team> _teams;		//The list of teams, indexed by name
        public int _playerCount = 0;

        public Arena(GameClient game)
        {
            _game = game;
            _players = new Dictionary<ushort, Player>();
            _teams = new Dictionary<string, Team>();

            createTeam("spec");
        }

        public void poll()
        {


        }
    }
}