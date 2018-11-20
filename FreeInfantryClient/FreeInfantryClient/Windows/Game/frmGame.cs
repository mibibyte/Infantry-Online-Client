using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using System.Windows.Forms;
using System.Net;
using FreeInfantryClient.Game;
using InfServer.Protocol;


namespace FreeInfantryClient.Windows
{
    public partial class Game : Form
    {
        static GameClient _game;
        private string _ticketid;
        private string _alias;

        public static void onException(object o, UnhandledExceptionEventArgs e)
        {	//Talk about the exception
            using (InfServer.LogAssume.Assume(_game._logger))
                InfServer.Log.write(InfServer.TLog.Exception, "Unhandled exception:\r\n" + e.ExceptionObject.ToString());
        }

        public Game(IPEndPoint serverLoc, string ticketid, string alias)
        {
            InitializeComponent();

            _ticketid = ticketid;
            _alias = alias;

            InfServer.Log.init();
            InfServer.DdMonitor.bNoSync = false;
            InfServer.DdMonitor.bEnabled = false;
            InfServer.DdMonitor.DefaultTimeout = 2000;

            //Register our catch-all exception handler
            Thread.GetDomain().UnhandledException += onException;

            //Create a logging client for the main client thread
            InfServer.LogClient handlerLogger = InfServer.Log.createClient("ClientHandler");
            InfServer.Log.assume(handlerLogger);

            //Create our client
            _game = new GameClient(this, alias, ticketid);

            //Connect
            _game.connect(serverLoc);

        }

        private void Game_Load(object sender, EventArgs e)
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }


        private void Game_Move(object sender, EventArgs e)
        {
            foreach (Control control in this.Controls)
            {
                if (control.Name.StartsWith("dialog"))
                {
                    control.Left = (this.ClientSize.Width - control.Width) / 2;
                    control.Top = (this.ClientSize.Height - control.Height) / 2;
                }
            }
        }

        private void Game_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
                MinimizeAllChildForms(this);
        }

        public void MinimizeAllChildForms(Form parent)
        {
            foreach (Form f in parent.OwnedForms)
            {
                f.WindowState = FormWindowState.Minimized;
                MinimizeAllChildForms(f);
            }
        }

        private void Game_FormClosing(object sender, FormClosingEventArgs e)
        {
            _game = null;
        }

        #region Form Effects
        public async void FadeIn(int interval = 80)
        {
            //Object is not fully invisible. Fade it in
            while (this.Opacity < 1.0)
            {
                await Task.Delay(interval);
                this.Opacity += 0.05;
            }
            this.Opacity = 1; //make fully visible       
        }

        public async void FadeOut(int interval = 80)
        {
            //Object is fully visible. Fade it out
            while (this.Opacity > 0.0)
            {
                await Task.Delay(interval);
                this.Opacity -= 0.05;
            }
            this.Opacity = 0; //make fully invisible
            this.Close();
            this.Dispose();
        }
        #endregion
    }
}
       