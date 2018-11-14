using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeInfantryClient
{
    public class Helpers
    {
        //Type of chat
        public enum Chat_Type
        {
            Normal = 0,
            Macro = 1,
            Whisper = 2,
            Team = 3,
            EnemyTeam = 4,
            Arena = 5,
            PrivateChat = 6,
            Squad = 7
        }
    }
}
