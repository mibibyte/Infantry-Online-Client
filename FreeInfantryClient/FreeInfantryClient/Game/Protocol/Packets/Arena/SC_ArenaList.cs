using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using InfServer.Network;
using FreeInfantryClient.Game;

namespace InfServer.Protocol
{   /// <summary>
    /// SC_ArenaList contains a list of arenas joinable by the player
    /// </summary>
    public class SC_ArenaList : PacketBase
    {   // Member Variables
        ///////////////////////////////////////////////////
        public List<Arena> arenalist;
        public Player requestee;

        public const ushort TypeID = (ushort)Helpers.PacketIDs.S2C.ArenaList;
        static public event Action<SC_ArenaList, Client> Handlers;




        ///////////////////////////////////////////////////
        // Member Functions
        //////////////////////////////////////////////////
        /// <summary>
        /// Creates an empty packet of the specified type. This is used
        /// for constructing new packets for sending.
        /// </summary>
        public SC_ArenaList()
            : base(TypeID)
        { }


        public SC_ArenaList(ushort typeID, byte[] buffer, int index, int count)
            : base(typeID, buffer, index, count)
        {
            arenalist = new List<Arena>();
        }



        public override void Route()
        {	//Call all handlers!
            if (Handlers != null)
                Handlers(this, (Client)_client);
        }


        public override void Deserialize()
        {
            Arena arena;
            //How many listings do we have? (+1 since our first typeID is lobbed off and it's assumed there is always 1 arena)
            int count = Data.Count(b => b == TypeID) + 1;

            if (count > 1)
            {
                for (int i = 0; i < count; i++)
                {
                    if (i > 0)
                        _contentReader.ReadChar();

                    arena = new Arena(null);
                    arena._name = ReadString(32);
                    arena._playerCount += Math.Abs(_contentReader.ReadInt16());

                    arenalist.Add(arena);

                }
            }
            else
            {
                arena = new Arena(null);
                arena._name = ReadString(32);
                arena._playerCount += Math.Abs(_contentReader.ReadInt16());

                arenalist.Add(arena);
            }
        }

        /// <summary>
        /// Serializes the data stored in the packet class into a byte array ready for sending.
        /// </summary>
        public override void Serialize()
        {   //Write out each asset
        }

        /// <summary>
        /// Returns a meaningful of the packet's data
        /// </summary>
        public override string Dump
        {
            get
            {
                return "Arena info.";
            }
        }
    }
}
