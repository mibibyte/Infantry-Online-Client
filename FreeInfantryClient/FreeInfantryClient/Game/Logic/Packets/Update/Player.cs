using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using InfServer.Network;
using InfServer.Protocol;
using FreeInfantryClient.Game.Protocol;
using FreeInfantryClient.Windows.Dialogs;
using System.Windows.Forms;

namespace FreeInfantryClient.Game.Logic
{
    public partial class PlayerUpdates
    {
        /// <summary>
        /// Handles the initial packet sent by the server
        /// </summary>
        static public void Handle_SC_PlayerWarp(SC_PlayerWarp pkt, Client client)
        {
            GameClient c = ((client as Client<GameClient>)._obj);
            c._player._state.positionX = pkt.topX;
            c._player._state.positionY = pkt.topY;
            c._player._state.health = 1337;
        }

        /// <summary>
        /// Registers all handlers
        /// </summary>
        [InfServer.Logic.RegistryFunc]
        static public void Register()
        {
            SC_PlayerWarp.Handlers += Handle_SC_PlayerWarp;
        }
    }
}
