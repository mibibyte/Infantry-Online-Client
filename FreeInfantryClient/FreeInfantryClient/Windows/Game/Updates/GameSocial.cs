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
using FreeInfantryClient.Settings;


namespace FreeInfantryClient.Windows
{
    //To keep this partial class from loading as a designer, kinda hacky, but it works.
    public partial class Dummy { }

    public partial class Game
    {
        //Updates our visual playerlist
        public void updatePlayerList(IEnumerable<Team> teams)
        {
            this.BeginInvoke((Action)(() =>
            {
                //Lazy way of doing this...
                lstPlayers.Nodes.Clear();

                //Sort our teams
                var sorted = teams.OrderBy(x => x._name == "spec" ? 0 : 1)
                    .ThenBy(x => x._name);

                foreach (Team team in sorted)
                {
                    int count = team._players.Count();

                    TreeNode teamNode = new TreeNode(String.Format("{0} ({1})", team._name, count));
                    foreach (KeyValuePair<ushort, Player> player in team._players)
                    {
                        TreeNode playerNode = new TreeNode(String.Format("{0}", player.Value._alias));
                        teamNode.Nodes.Add(playerNode);
                    }
                    lstPlayers.Nodes.Add(teamNode);
                }
            }));
        }

        //Updates our visual chat log
        public void updateDeath(string killer, string victim, InfServer.Protocol.Helpers.KillType type)
        {
            switch (type)
            {
                case InfServer.Protocol.Helpers.KillType.Player:
                    {
                        AppendText(String.Format("{0} killed by {1}", victim, killer), Color.Yellow, true);
                    }
                    break;
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
                            AppendText(String.Format("{0}> {1}", from, message), Color.Khaki, true);
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


        private void btnSend_Click(object sender, EventArgs e)
        {
            //No empty messages
            if (chatSend.Text == "")
                return;

            //Chat message?
            if (chatSend.Text.StartsWith(";"))
            {

                string[] chatMsg = chatSend.Text.Split(';');
                int index = 1;
                string message = "";

                if (chatMsg.Count() == 3)
                {
                    Int32.TryParse(chatMsg[1], out index);
                    message = chatMsg[2];
                }
                else
                    message = chatMsg[1];

                //No chats loaded or chat index higher than number of chats?
                if (GameSettings.Chats._chats.Count == 0 || index > GameSettings.Chats._chats.Count)
                {
                    chatSend.Clear();
                    return;
                }

                string chat;
                chat = GameSettings.Chats._chats[index - 1];

                _game.sendChat(message, chat, InfServer.Protocol.Helpers.Chat_Type.PrivateChat);
                chatSend.Clear();

                return;
            }


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
    }
}
