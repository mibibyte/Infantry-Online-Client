using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfServer;
using InfServer.Protocol;

namespace FreeInfantryClient.Game.Logic
{
    public partial class State
    {   /// <summary>
        /// Handles a player enter packet received from the server
        /// </summary>
        static public void Handle_SC_Players(SC_PlayerEnter pkt, Client client)
        {
            GameClient c = ((client as Client<GameClient>)._obj);

            c._arena.playerEnter(pkt);
               
        }

        static public void Handle_SC_PlayerLeave(SC_PlayerLeave pkt, Client client)
        {
            GameClient c = ((client as Client<GameClient>)._obj);

            c._arena.playerLeave(pkt.playerID);

        }

        static public void Handle_SC_ChangeTeam(SC_ChangeTeam pkt, Client client)
        {
            GameClient c = ((client as Client<GameClient>)._obj);
            c._arena.playerChangeTeam(pkt.playerID, pkt.teamname);
        }

        /// <summary>
        /// Registers all handlers
        /// </summary>
        [InfServer.Logic.RegistryFunc]
        static public void Register()
        {
            SC_PlayerEnter.Handlers += Handle_SC_Players;
            SC_ChangeTeam.Handlers += Handle_SC_ChangeTeam;
            SC_PlayerLeave.Handlers += Handle_SC_PlayerLeave;
        }
    }

}