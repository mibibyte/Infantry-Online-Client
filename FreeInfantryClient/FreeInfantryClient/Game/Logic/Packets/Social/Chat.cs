using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Threading;
using FreeInfantryClient.Windows;
using InfServer.Protocol;
using FreeInfantryClient.Game;

namespace FreeInfantryClient.Game.Protocol
{
    public partial class Chat
    {
        /// <summary>
        /// Handles a chat packet received from the server
        /// </summary>
        static public void Handle_SC_Chat(SC_Chat pkt, Client client)
        {
            GameClient c = ((client as Client<GameClient>)._obj);

            switch (pkt.chatType)
            {
                case InfServer.Protocol.Helpers.Chat_Type.Arena:
                    {
                        c._wGame.updateChat( pkt.message, pkt.from, pkt.chatType, "");
                    }
                    break;

                case InfServer.Protocol.Helpers.Chat_Type.Normal:
                    {
                        c._wGame.updateChat(pkt.message, pkt.from, pkt.chatType, "");
                    }
                    break;

                case InfServer.Protocol.Helpers.Chat_Type.Team:
                    {
                        c._wGame.updateChat(pkt.message, pkt.from, pkt.chatType, "");
                    }
                    break;

                case InfServer.Protocol.Helpers.Chat_Type.Whisper:
                    {
                        c._wGame.updateChat(pkt.message, pkt.from, pkt.chatType, "");
                    }
                    break;
                case InfServer.Protocol.Helpers.Chat_Type.PrivateChat:
                    {
                        c._wGame.updateChat(pkt.message, pkt.from, pkt.chatType, pkt.chat);
                    }
                    break;

                default:
                    {
                        c._wGame.updateChat("ZoneServer", pkt.message, pkt.chatType, "");
                    }
                    break;
            }

        }

        /// <summary>
        /// Registers all handlers
        /// </summary>
        [InfServer.Logic.RegistryFunc]
        static public void Register()
        {
            SC_Chat.Handlers += Handle_SC_Chat;
        }
    }
}
