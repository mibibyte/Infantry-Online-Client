using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Assets;
using InfServer.Network;

namespace InfServer.Protocol
{	/// <summary>
	/// SC_AssetInfo contains info for all the infantry assets, with their 
	/// associated checksums.
	/// </summary>
	public class SC_AssetInfo : PacketBase
	{	// Member Variables
		///////////////////////////////////////////////////
		public bool bOptionalUpdate;

		public const ushort TypeID = (ushort)Helpers.PacketIDs.S2C.AssetInfo;
        static public event Action<SC_AssetInfo, Client> Handlers;


		///////////////////////////////////////////////////
		// Member Functions
		//////////////////////////////////////////////////
		/// <summary>
		/// Creates an empty packet of the specified type. This is used
		/// for constructing new packets for sending.
		/// </summary>
		public SC_AssetInfo()
			: base(TypeID)
		{
		}

        public SC_AssetInfo(ushort typeID, byte[] buffer, int index, int count)
			: base(typeID, buffer, index, count)
		{
		}

        /// <summary>
        /// Routes a new packet to various relevant handlers
        /// </summary>
        public override void Route()
        {	//Call all handlers!
            if (Handlers != null)
                Handlers(this, (Client)_client);
        }

		/// <summary>
		/// Serializes the data stored in the packet class into a byte array ready for sending.
		/// </summary>
		public override void Serialize()
		{

				Write((byte)TypeID);
				return;
		}

        public override void Deserialize()
        {
           
        }

		/// <summary>
		/// Returns a meaningful of the packet's data
		/// </summary>
		public override string Dump
		{
			get
			{
				return "Asset info,  files.";
			}
		}
	}
}
