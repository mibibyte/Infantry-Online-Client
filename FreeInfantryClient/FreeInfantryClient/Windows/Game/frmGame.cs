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

        //Updates our visual playerlist
        public void updatePlayerList(Dictionary<ushort, Player> players)
        {
            //Lazy way of doing this...
            lstPlayers.Items.Clear();

            var sorted = players.OrderBy(x => x.Value._team == "spec" ? 0 : 1)
            .ThenBy(x => x.Value._team);

            foreach (KeyValuePair<ushort, Player> player in sorted)
            {
                lstPlayers.Items.Add(String.Format("{0} ({1})", player.Value._alias, player.Value._team));
            }
        }

        //Updates our visual chat log
        public void updateChat(string message, string from, InfServer.Protocol.Helpers.Chat_Type type, string chat)
        {


            switch (type)
            {
                case InfServer.Protocol.Helpers.Chat_Type.System:
                    {
                        AppendText(String.Format("{0}> {1}", from, message), Color.GhostWhite, true);
                    }
                    break;

                case InfServer.Protocol.Helpers.Chat_Type.Arena:
                    {
                        Color color = Color.GreenYellow;

                        if (message.Length > 0)
                        {
                            string prefix = message.Substring(0, 1);

                            switch (prefix)
                            {
                                case "!":
                                    color = Color.Red;
                                    break;
                                case "@":
                                    color = Color.Red;
                                    break;
                                case "#":
                                    color = Color.MediumVioletRed;
                                    break;
                                case "$":
                                    color = Color.MediumVioletRed;
                                    break;
                                case "%":
                                    color = Color.Blue;
                                    break;
                                case "^":
                                    color = Color.Blue;
                                    break;
                                case "&":
                                    color = Color.FloralWhite;
                                    break;
                                case "*":
                                    color = Color.FloralWhite;
                                    break;


                                default:
                                    break;
                            }
                        }

                        AppendText(String.Format("{0}> {1}", from, message), color, true);
                    }
                    break;

                case InfServer.Protocol.Helpers.Chat_Type.Normal:
                    {
                        AppendText(String.Format("{0}> {1}", from, message), Color.Aqua, true);
                    }
                    break;

                case InfServer.Protocol.Helpers.Chat_Type.Team:
                    {
                        AppendText(String.Format("{0}> {1}", from, message), Color.Yellow, true);
                    }
                    break;

                case InfServer.Protocol.Helpers.Chat_Type.Whisper:
                    {
                        AppendText(String.Format("{0}> {1}", from, message), Color.GreenYellow, true);
                    }
                    break;

                case InfServer.Protocol.Helpers.Chat_Type.PrivateChat:
                    {
                        string[] chatmsg = message.Split(':');
                        if (chatmsg.Count() == 2)
                        AppendText(String.Format("{0}: {1}> {2}", chatmsg[0], from, chatmsg[1]), Color.Khaki, true);
                        else
                            AppendText(String.Format("{0}> {1}", from, message), Color.GreenYellow, true);
                    }
                    break;

                default:
                    {
                        AppendText(String.Format("{0}> {1}", from, message), Color.Aqua, true);
                    }
                    break;
            }
        }

        public void AppendText(string text, Color color, bool addNewLine = false)
        {
            chatLog.SuspendLayout();
            chatLog.SelectionColor = color;
            chatLog.AppendText(addNewLine
                ? $"{text}{Environment.NewLine}"
                : text);
            chatLog.ScrollToCaret();
            chatLog.ResumeLayout();
        }

        private void Game_Load(object sender, EventArgs e)
        {

        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            //No empty messages
            if (chatSend.Text == "")
                return;

            //Private message?
            if (chatSend.Text.StartsWith(":"))
            {
                string[] privateMsg = chatSend.Text.TrimStart(':').Split(':');

                //Sanity check
                if (privateMsg.Count() > 1)
                {
                    _game.sendChat(privateMsg[1], privateMsg[0], InfServer.Protocol.Helpers.Chat_Type.Whisper);
                    _game._player._lastPM = privateMsg[0];
                    chatSend.Clear();
                    return;
                }
            }

            //Team Message?
            if (chatSend.Text.StartsWith("'"))
            {
                //Trim our prefix
                string teamMsg = chatSend.Text.TrimStart('\'');
                //Send it
                _game.sendChat(teamMsg, "", InfServer.Protocol.Helpers.Chat_Type.Team);
                chatSend.Clear();
                return;
            }


            //Chat command?
            string command = "";
            string payload = "";
            if (chatSend.Text.StartsWith("?"))
            {
                int spcIdx = chatSend.Text.IndexOf(' ');
                //Do we have a payload?
                if (spcIdx == -1)
                    command = chatSend.Text.Substring(1);
                else
                {
                    command = chatSend.Text.Substring(1, spcIdx - 1);
                    payload = chatSend.Text.Substring(spcIdx + 1);
                }
                FreeInfantryClient.Game.Commands.HandlerDescriptor handler;

                if (_game._commandRegistrar._chatCommands.ContainsKey(command))
                {
                     handler = _game._commandRegistrar._chatCommands[command];

                    //Handle it!
                    _game.playerChatCommand(_game._player, null, command, payload, 0);

                    //Should we pass it along to the server?
                    if (!handler.relay)
                    {
                        //Clear our message box
                        chatSend.Clear();
                        return;
                    }
                }

   

            }
            //Relay it to the server
            if (_game.IsConnected)
                _game.sendChat(chatSend.Text, "", InfServer.Protocol.Helpers.Chat_Type.Normal);

            //Clear our message box
            chatSend.Clear();
        }

        //Auto-PM
        private void chatSend_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Shift && e.KeyCode == Keys.OemSemicolon && chatSend.Text == ":")
            {
                if (_game._player._lastPM != null)
                {
                    chatSend.Clear();
                    chatSend.Text = String.Format(":{0}: ", _game._player._lastPM);
                    chatSend.SelectionStart = chatSend.Text.Length;
                }

            }
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
       