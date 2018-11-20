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
    public partial class Vehicles
    {
        /// <summary>
        /// Handles the initial packet sent by the server
        /// </summary>
        static public void Handle_SC_VehicleDeath(SC_VehicleDeath pkt, Client client)
        {
            GameClient c = ((client as Client<GameClient>)._obj);

            Player killer = c._arena._players[(ushort)pkt.killerID];
            Player victim = c._arena._players[(ushort)pkt.playerID];

            c._wGame.updateDeath(killer._alias, victim._alias, pkt.type);
        }

        /// <summary>
        /// Registers all handlers
        /// </summary>
        [InfServer.Logic.RegistryFunc]
        static public void Register()
        {
            SC_VehicleDeath.Handlers += Handle_SC_VehicleDeath;
        }
    }
}
