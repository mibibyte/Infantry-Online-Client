using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfServer.Logic;
using InfServer.Protocol;
using InfServer.Network;
using FreeInfantryClient.Settings;

namespace FreeInfantryClient.Game.Commands
{
    public class Chat
    {
        /// <summary>
        /// Allows a user to switch arenas
        /// </summary>
        public static void go(Player player, Player recipient, string payload, int bong)
        {
            player._gameclient.joinArena(payload);
        }

        /// <summary>
        /// Displays available arenas to the user
        /// </summary>
        public static void arena(Player player, Player recipient, string payload, int bong)
        {
        }

        /// <summary>
        /// Join/leave chats
        /// </summary>
        public static void chat(Player player, Player recipient, string payload, int bong)
        {
           
            //Ignore empty ?chat
            if (string.IsNullOrEmpty(payload))
                return;


            //Are we turning chats off?
            if (payload == "off")
            {
                //Leave all chats
                GameSettings.Chats._chats.Clear();
                return;
            }

            //Split it
            string[] chats = payload.Split(',');

            if (chats.Count() == 0)
                return;

            if (chats.Count() > 6)
                return;



            //Clear it first
            GameSettings.Chats._chats.Clear();

            //Readd them in the order specified
            foreach (string chat in chats)
                GameSettings.Chats._chats.Add(chat);
        }

        /// <summary>
        /// Join/leave chats
        /// </summary>
        public static void chatadd(Player player, Player recipient, string payload, int bong)
        {

            if (GameSettings.Chats._chats.Count() == 6)
                return;

            if (string.IsNullOrEmpty(payload))
                return;

            GameSettings.Chats._chats.Add(payload);

            //Reload
            player.loadChats();
        }

        /// <summary>
        /// Join/leave chats
        /// </summary>
        public static void chatdrop(Player player, Player recipient, string payload, int bong)
        {
            if (GameSettings.Chats._chats.Count() == 0)
                return;

            if (string.IsNullOrEmpty(payload))
                return;

            if (!GameSettings.Chats._chats.Contains(payload))
                return;

            GameSettings.Chats._chats.Remove(payload);

            //Reload
            player.loadChats();
        }

        /// <summary>
        /// Displays available arenas to the user
        /// </summary>
        public static void quit(Player player, Player recipient, string payload, int bong)
        {
            player._gameclient.quit();
        }

        /// <summary>
        /// Gives the user help information on a given command
        /// </summary>
        static public void help(Player player, Player recipient, string payload, int bong)
        {
            if (string.IsNullOrEmpty(payload))
            {	//List all mod commands
                player._gameclient._wGame.updateChat("Commands available to you:", "System", InfServer.Protocol.Helpers.Chat_Type.System, "");

                SortedList<string, int> commands = new SortedList<string, int>();

                //First set list
                foreach (HandlerDescriptor cmd in player._gameclient._commandRegistrar._chatCommands.Values)
                    player._gameclient._wGame.updateChat(String.Format("Command: {0} [Usage={1}]", cmd.handlerCommand, cmd.usage),
                        "System", InfServer.Protocol.Helpers.Chat_Type.System, "");
                return;
            }

            //Attempt to find the command's handler
            HandlerDescriptor handler;

            if (!player._gameclient._commandRegistrar._chatCommands.TryGetValue(payload.ToLower(), out handler))
            {
                player._gameclient._wGame.updateChat("Unable to find the specified command.", "System",
                    InfServer.Protocol.Helpers.Chat_Type.System, "");
                return;
            }

            //Display help information
            player._gameclient._wGame.updateChat("?" + handler.handlerCommand + ": " + handler.commandDescription,
            "System", InfServer.Protocol.Helpers.Chat_Type.System, "");
            player._gameclient._wGame.updateChat("Usage: " + handler.usage,
            "System", InfServer.Protocol.Helpers.Chat_Type.System, "");

        }


        #region Registrar
        /// <summary>
        /// Registers all handlers
        /// </summary>
        [Commands.RegistryFunc(HandlerType.ChatCommand)]
        public static IEnumerable<Commands.HandlerDescriptor> Register()
        {
            yield return new HandlerDescriptor(go, "go",
                "Switches a users current arena",
                "?go or ?go new arena", true);

            yield return new HandlerDescriptor(chat, "chat",
                "Joins/Leaves specified chats",
                "?chat chat1,chat2,chat3", true);

            yield return new HandlerDescriptor(chatadd, "chatadd",
                "Joins specified chat",
                "?chatadd chat", true);

            yield return new HandlerDescriptor(chatdrop, "chatdrop",
                "Leaves specified chat",
                "?chatdrop chat", true);

            yield return new HandlerDescriptor(quit, "quit",
                "Leaves the current zone and brings the user back to the zonelist",
                "?quit", false);

            yield return new HandlerDescriptor(arena, "arena",
                "Displays a list of available arenas",
                "?arena", true);

            yield return new HandlerDescriptor(help, "help",
                "Gives the user help information on a given command.",
                "?help [commandName]", false);
        }
        #endregion
    }
}
