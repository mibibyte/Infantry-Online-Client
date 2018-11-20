using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using InfServer.Network;

namespace InfServer.Protocol
{   /// <summary>
    /// SC_VehicleDeath is used to inform clients of a player's death
    /// </summary>
    public class SC_VehicleDeath : PacketBase
    {   // Member Variables
        ///////////////////////////////////////////////////
        public Helpers.KillType type;   //The way in which the player was killed
        public UInt16 playerID;         //The id of the player which was killed
        public UInt16 vehicleID;        //The id of the vehicle which was killed
        public UInt32 killerID;         //Villainous murdering scum!
        public Int16 points;            //Points earned by the killer
        public Int16 personalPoints;    //Points earned
        public Int16 personalExp;       //Experience earned
        public Int16 personalCash;      //Cash earned
        public Int16 positionX;
        public Int16 positionY;
        public Int16 positionZ;
        public byte yaw;
        public byte unk1;

        public const ushort TypeID = (ushort)Helpers.PacketIDs.S2C.VehicleDeath;
        static public event Action<SC_VehicleDeath, Client> Handlers;



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
        public SC_VehicleDeath()
            : base(TypeID)
        { }


        public SC_VehicleDeath(ushort typeID, byte[] buffer, int index, int count)
            : base(typeID, buffer, index, count)
        {
        }

        public override void Deserialize()
        {
            type = (Helpers.KillType)_contentReader.ReadByte();
            playerID = _contentReader.ReadUInt16();
            vehicleID = _contentReader.ReadUInt16();
            killerID = _contentReader.ReadUInt16();
        }

        /// <summary>
        /// Serializes the data stored in the packet class into a byte array ready for sending.
        /// </summary>
        public override void Serialize()
        {
            Write((byte)TypeID);

            Write((byte)type);
            Write(playerID);
            Write(vehicleID);
            Write(killerID);
            Write(points);
            Write(personalPoints);
            Write(personalExp);
            Write(personalCash);
            Write(positionX);
            Write(positionY);
            Write(positionZ);
            Write(yaw);
            Write(unk1);
        }

        /// <summary>
        /// Returns a meaningful of the packet's data
        /// </summary>
        public override string Dump
        {
            get
            {
                return "Player death notification.";
            }
        }
    }
}
