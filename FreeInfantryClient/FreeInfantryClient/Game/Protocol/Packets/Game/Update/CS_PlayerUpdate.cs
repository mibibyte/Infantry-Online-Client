using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using InfServer.Network;
using FreeInfantryClient.Game;

using Assets;


namespace InfServer.Protocol
{   /// <summary>
    /// CS_PlayerUpdate is sent by the client to update the player's
    /// position and status.
    /// </summary>
    public class CS_PlayerUpdate : PacketBase
    {   // Member Variables
        ///////////////////////////////////////////////////
        public bool bIgnored;               //Was the update ignored?

        public Int16 energy;                //Current energy
        public UInt16 itemCRC;              //The CRC checksum of the item used
        public UInt16 tickCount;            //The tick count at the time update was made
        public UInt16 playerID;             //The player's current id
        public Int16 itemID;                //The ID of the item the player is using, if any
        public Int32 fullTickCount;         //The full client's tickcount (only to be used relatively)
        public Int16 health;                //Player's health
        public Int16 velocityX;             //Velocity info
        public Int16 velocityY;             //
        public Int16 velocityZ;             //
        public Int16 positionX;             //Positional info
        public Int16 positionY;             //
        public Int16 positionZ;             //
        public byte yaw;                    //Our rotation
        public UInt16 direction;            //The direction we're attempting to move on
        public byte unk1;                   //Unknown (flags?) Has to do with viewing the player (in game or not)

        public byte pitch;                  //Pitch, for dependent vehicles

        //Spectator
        public Int16 playerSpectating;      //The ID of the player we're spectating

        public List<ushort> activeEquip;    //Any additional equipment we may have active

        //Packet routing
        public const ushort TypeID = (ushort)Helpers.PacketIDs.C2S.PlayerUpdate;
        static public Action<CS_PlayerUpdate, Client> Handlers;


        ///////////////////////////////////////////////////
        // Member Functions
        //////////////////////////////////////////////////
        /// <summary>
        /// Creates an empty packet of the specified type. This is used
        /// for constructing new packets for sending.
        /// </summary>
        public CS_PlayerUpdate()
            : base(TypeID)
        {
        }


        ///////////////////////////////////////////////////
        // Member Functions
        //////////////////////////////////////////////////
        /// <summary>
        /// Creates an instance of the dummy packet used to debug communication or 
        /// to represent unknown packets.
        /// </summary>
        /// <param name="typeID">The type of the received packet.</param>
        /// <param name="buffer">The received data.</param>
        public CS_PlayerUpdate(ushort typeID, byte[] buffer, int index, int count)
            : base(typeID, buffer, index, count)
        {
        }

        /// <summary>
        /// Routes a new packet to various relevant handlers
        /// </summary>
        public override void Route()
        {   //Call all handlers!
            if (Handlers != null)
                Handlers(this, ((Client)_client));
        }


        public override void Serialize()
        {
            Write((byte)TypeID);

            Write(energy);
            Write(itemCRC);
            Write(tickCount);
            Write(playerID);
            Write(itemID);
            Write(fullTickCount);
            Write(health);
            Write(velocityX);
            Write(velocityY);
            Write(velocityZ);
            Write(positionX);
            Write(positionY);
            Write(positionZ);
            Write(yaw);
            Write(direction);
            Write(unk1);
        }

        /// <summary>
        /// Deserializes the data present in the packet contents into data fields in the class.
        /// </summary>
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
                return "Player update";
            }
        }
    }
}
