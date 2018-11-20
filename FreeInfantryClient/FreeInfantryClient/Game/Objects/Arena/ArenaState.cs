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
    public partial class Arena : CustomObject
    {

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

        #region Team Accessors

        /// <summary>
        /// Returns a list of teams present in the arena
        /// </summary>
        public IEnumerable<Team> Teams
        {
            get
            {
                return _teams.Values;
            }
        }

        /// <summary>
        /// Obtains a team by name
        /// </summary>
        public Team getTeamByName(string name)
        {	//Attempt to find it
            Team team;

            if (_teams.TryGetValue(name.ToLower(), out team))
                //Check _teams
                return team;
            foreach (Team arenaTeam in Teams)
                //Check Teams
                if (arenaTeam._name.ToLower() == name.ToLower())
                    return arenaTeam;
            //Still nothing? team doesn't exist
            return null;
        }

        /// <summary>
        /// Creates a team within the arena, returns null if it already exists
        /// </summary>
        public Team createTeam(string team)
        {
            Team newTeam;

            newTeam = new Team(this, _game);
            newTeam._name = team;

            _teams.Add(team, newTeam);

            return newTeam;
        }
        #endregion
    }
}