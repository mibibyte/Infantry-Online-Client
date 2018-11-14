using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using InfServer.Network;

using Assets;

namespace FreeInfantryClient.Game
{   /// <summary>
    /// Provides a series of functions for easily serialization of packets
    /// </summary>
    public partial class Helpers
    {   // Member Classes
        //////////////////////////////////////////////////
        /// <summary>
        /// Contains positional and other information an object
        /// </summary>
        public class ObjectState
        {
            public Int16 health;
            public Int16 energy;

            public Int16 velocityX;         //Velocity info
            public Int16 velocityY;         //
            public Int16 velocityZ;         //

            public Int16 positionX;         //Positional info
            public Int16 positionY;         //
            public Int16 positionZ;         //

            public byte yaw;                //Our rotation
            public Direction direction;     //The direction we're attempting to move in
            public byte unk1;               //Unknown (flags?)

            public byte pitch;

            //
            public byte fireAngle;          //Used for computer vehicles
            public int lastUpdate;          //The last point this state was updated
            public int lastUpdateServer;    //The time at which this was received on the server
            public int updateNumber;        //The update counter, used for route range factoring

            //Assume the direction system is hardcoded for now
            public enum Direction : ushort
            {
                None = 0,
                Forward = 0x3C,
                Backward = 0xD3,
                StrafeLeft = 0xBF00,
                StrafeRight = 0x4100,
                NorthWest = 0xD32A,
                SouthWest = 0xD3E1,
                SouthEast = 0x2DE1,
                NorthEast = 0x2D2A,
            }

        }
    }
}
