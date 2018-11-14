using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using InfServer.Network;

namespace InfServer.Protocol
{   /// <summary>
    /// SC_PlayerLeave notifies of another player leaving the arena
    /// </summary>
    public class SC_PlayerLeave : PacketBase
    {   // Member Variables
        ///////////////////////////////////////////////////
        public UInt16 playerID;

        public const ushort TypeID = (ushort)Helpers.PacketIDs.S2C.PlayerLeave;
        static public event Action<SC_PlayerLeave, Client> Handlers;


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
        public SC_PlayerLeave()
            : base(TypeID)
        { }


        public SC_PlayerLeave(ushort typeID, byte[] buffer, int index, int count)
            : base(typeID, buffer, index, count)
        {
        }

        public override void Deserialize()
        {
            playerID = _contentReader.ReadUInt16();
        }
        /// <summary>
        /// Serializes the data stored in the packet class into a byte array ready for sending.
        /// </summary>
        public override void Serialize()
        {   //Typeid
            Write((byte)TypeID);

            Write(playerID);
        }



        /// <summary>
        /// Returns a meaningful of the packet's data
        /// </summary>
        public override string Dump
        {
            get
            {
                return "Player leaving";
            }
        }
    }
}
