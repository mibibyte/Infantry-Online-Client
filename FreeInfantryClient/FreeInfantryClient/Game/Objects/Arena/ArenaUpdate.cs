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
    {

        #region Player Events
        public void playerLeave(ushort id)
        {   //Who are we talking about?
            Player player = getPlayerById(id);
            //Leave his team aswell
            player._team.playerLeave(id);
            _players.Remove(id);

            //Update our GUI
            _game._wGame.updatePlayerList(Teams.Where(t => t._players.Count > 0));
        }

        public void playerEnter(SC_PlayerEnter pkt)
        {
            //Create a new player instance
            Player player = new Player(false);
            player._alias = pkt.alias;
            player._id = pkt.id;

            if (getTeamByName(pkt.teamname) == null)
                player._team = createTeam(pkt.teamname);

            player._team = getTeamByName(pkt.teamname);

            player._team.playerJoin(player);

            //Update our dictionary
            _players.Add(player._id, player);

            //Update our GUI
            _game._wGame.updatePlayerList(Teams.Where(t => t._players.Count > 0));
        }

        public void playerChangeTeam(ushort playerID, string teamname)
        {
            Player player = getPlayerById(playerID);
            Team newTeam = getTeamByName(teamname);

            if (newTeam == null)
                newTeam = createTeam(teamname);

            newTeam.playerJoin(player);

            //Update our GUI
            _game._wGame.updatePlayerList(Teams.Where(t => t._players.Count > 0));
        }
        #endregion

    }
}