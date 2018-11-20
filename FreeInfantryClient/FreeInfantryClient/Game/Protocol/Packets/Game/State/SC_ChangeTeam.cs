using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using InfServer.Network;

namespace InfServer.Protocol
{	/// <summary>
	/// SC_ChangeTeam notifies a client of a forced team change
	/// </summary>
	public class SC_ChangeTeam : PacketBase
	{	// Member Variables
		///////////////////////////////////////////////////
		public UInt16 playerID;
		public Int16 teamID;
		public string teamname;

		public const ushort TypeID = (ushort)Helpers.PacketIDs.S2C.ChangeTeam;
        static public event Action<SC_ChangeTeam, Client> Handlers;


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
        public SC_ChangeTeam()
            : base(TypeID)
        { }


        public SC_ChangeTeam(ushort typeID, byte[] buffer, int index, int count)
            : base(typeID, buffer, index, count)
        {
        }

        public override void Deserialize()
        {

            playerID = _contentReader.ReadUInt16();
            teamID = _contentReader.ReadInt16();
            teamname = ReadString(32);

        }

        /// <summary>
        /// Serializes the data stored in the packet class into a byte array ready for sending.
        /// </summary>
        public override void Serialize()
		{	//Type ID
			Write((byte)TypeID);

			//Contents
			Write(playerID);
			Write(teamID);
			Write(teamname, 32);
		}

		/// <summary>
		/// Returns a meaningful of the packet's data
		/// </summary>
		public override string Dump
		{
			get
			{
				return "Team change to '" + teamname + "'";
			}
		}
	}
}
