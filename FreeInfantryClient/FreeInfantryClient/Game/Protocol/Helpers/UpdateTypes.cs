using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfServer.Protocol
{
        public partial class Helpers
        {   // Member Classes
            //////////////////////////////////////////////////
            //Type of object update
            public enum Update_Type
            {
                Car = 5,
            }

            public enum ResetFlags
            {
                ResetNone = 0,
                ResetEnergy = 1,
                ResetHealth = 2,
                ResetVelocity = 4,
                ResetAll = 7,
            };

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
                Squad = 7,
                System = 8
            }

            //Ways in which players can be killed
            public enum KillType
            {
                Player,
                Computer,
                Terrain,
                Flag,
                Prize,
                Explosion,
                Test,
            }
        }
}
