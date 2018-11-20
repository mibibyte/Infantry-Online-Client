using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using InfServer.Network;

namespace InfServer.Protocol
{   /// <summary>
    /// SC_PlayerWarp is used to force the client to warp to a location 
    /// </summary>
    public class SC_PlayerWarp : PacketBase
    {   // Member Variables
        ///////////////////////////////////////////////////
        public Helpers.ResetFlags warpFlags;        //Type of warp
        public Int16 invulnTime;    //The amount of time we're invulnerable after spawning
        public Int16 bottomX;       //The warp window
        public Int16 bottomY;       //
        public Int16 topX;          //
        public Int16 topY;          //
        public Int16 energy;

        public const ushort TypeID = (ushort)Helpers.PacketIDs.S2C.PlayerWarp;
        static public event Action<SC_PlayerWarp, Client> Handlers;



        public override void Route()
        {	//Call all handlers!
            if (Handlers != null)
                Handlers(this, (Client)_client);
        }


        ///////////////////////////////////////////////////
        // Member Functions
        //////////////////////////////////////////////////
        /// <summary>
        /// Creates an empty packet of the specified type. This is used
        /// for constructing new packets for sending.
        /// </summary>
        public SC_PlayerWarp()
            : base(TypeID)
        { }


        public SC_PlayerWarp(ushort typeID, byte[] buffer, int index, int count)
            : base(typeID, buffer, index, count)
        {
        }

        public override void Deserialize()
        {
            warpFlags = (Helpers.ResetFlags)_contentReader.ReadByte();
            invulnTime = _contentReader.ReadInt16();
            topX = _contentReader.ReadInt16();
            topY = _contentReader.ReadInt16();
            bottomX = _contentReader.ReadInt16();
            bottomY = _contentReader.ReadInt16();
            energy = _contentReader.ReadInt16();
        }

        /// <summary>
        /// Serializes the data stored in the packet class into a byte array ready for sending.
        /// </summary>
        public override void Serialize()
        {
            Write((byte)TypeID);

            Write((byte)warpFlags);
            Write(invulnTime);
            Write(topX);
            Write(topY);
            Write(bottomX);
            Write(bottomY);
            Write(energy);
        }

        /// <summary>
        /// Returns a meaningful of the packet's data
        /// </summary>
        public override string Dump
        {
            get
            {
                return "Player warp notification";
            }
        }
    }
}
