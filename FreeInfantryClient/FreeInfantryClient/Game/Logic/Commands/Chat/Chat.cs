using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfServer.Logic;
using InfServer.Protocol;
using InfServer.Network;

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
