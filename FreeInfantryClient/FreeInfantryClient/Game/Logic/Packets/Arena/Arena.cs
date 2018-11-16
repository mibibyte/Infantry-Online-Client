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
    public partial class Arenas
    {
        /// <summary>
        /// Handles the initial packet sent by the server
        /// </summary>
        static public void Handle_SC_ArenaList(SC_ArenaList pkt, Client client)
        {
            GameClient c = ((client as Client<GameClient>)._obj);




            //Open a new Arena List Dialog
            dialog_ArenaList arenaList = new dialog_ArenaList(pkt.arenalist, c);
            c._wGame.Invoke((MethodInvoker)delegate ()
            {

                arenaList.TopLevel = false;
                c._wGame.Controls.Add(arenaList);
                arenaList.Show();

                //Center it
                arenaList.Left = (c._wGame.ClientSize.Width - arenaList.Width) / 2;
                arenaList.Top = (c._wGame.ClientSize.Height - arenaList.Height) / 2;

                //And bring it forward
                arenaList.BringToFront();
                arenaList.showArenas();

            });
        }

        /// <summary>
        /// Registers all handlers
        /// </summary>
        [InfServer.Logic.RegistryFunc]
        static public void Register()
        {
            SC_ArenaList.Handlers += Handle_SC_ArenaList;
        }
    }
}
