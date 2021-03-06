﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FreeInfantryClient.Game;

using InfServer.Network;

namespace InfServer.Protocol
{	/// <summary>
	/// SC_PlayerEnter contains updates regarding players entering the arena 
	/// </summary>
	public class SC_PlayerEnter : PacketBase
	{	// Member Variables
		///////////////////////////////////////////////////
        public string teamname;
        public string alias;
        public string squad;
        public ushort id;

		public IEnumerable<Player> players;

		public const ushort TypeID = (ushort)Helpers.PacketIDs.S2C.PlayerEnter;
        static public event Action<SC_PlayerEnter, Client> Handlers;


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
        public SC_PlayerEnter()
            : base(TypeID)
        { }


        public SC_PlayerEnter(ushort typeID, byte[] buffer, int index, int count)
            : base(typeID, buffer, index, count)
        {
        }

        public override void Deserialize()
        {
            teamname = ReadString(32);
            alias = ReadString(32);
            squad = ReadString(32);
            id = _contentReader.ReadUInt16();
         
        }

		/// <summary>
		/// Returns a meaningful of the packet's data
		/// </summary>
		public override string Dump
		{
			get
			{
				return "Player info.";
			}
		}

    }
}
