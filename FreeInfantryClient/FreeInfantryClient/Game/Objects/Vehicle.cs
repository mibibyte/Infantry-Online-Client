using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

using InfServer.Network;
using InfServer.Protocol;

using Assets;

namespace FreeInfantryClient.Game
{
    // Vehicle Class
    /// Represents a single vehicle in an arena
    ///////////////////////////////////////////////////////
    public class Vehicle : CustomObject, ILocatable
    {	// Member variables
        ///////////////////////////////////////////////////
        public bool bCondemned;				//Is the vehicle ready to be deleted?
        public Arena _arena;				//The arena we belong to
        public VehInfo _type;				//The type of vehicle we represent


        public Team _team;					//The team we belong to
        public Player _creator;				//The player which created us
        public Team _oldTeam;               //Our original owners
        public bool _reprogrammed;          //Did this vehicle get reprogrammed?

        public List<Vehicle> _childs;		//Our child vehicles
        public Vehicle _parent;				//Our parent vehicle, if we're a dependent vehicle
        public int _parentSlot;				//The slot we occupy in the parent vehicle

        public bool _bBotVehicle;			//Are we a bot-owned vehicle?
        public bool _bBaseVehicle;			//Are we a base vehicle, implied by a player?
        public Player _inhabitant;			//The player, if any, that's inside the vehicle
        public int _lane;

        public ushort _id;					//Our vehicle ID

        private int _relativeID;            //Relative ID of the vehicle, can be overridden
        public int relativeID               //when the vehicle spawns
        {
            get { return (_relativeID == 0 ? _type.RelativeId : _relativeID); }
            set { _relativeID = value; }
        }

        #region Game state
        public Helpers.ObjectState _state;	//The state of our vehicle!
        public List<Player> _attackers;		//The list of players which have damaged this vehicle

        //Game timers
        public int _tickCreation;			//The time at which the vehicle was created
        public int _tickUnoccupied;			//The time at which the vehicle was last unoccupied
        public int _tickDead;				//The time at which the vehicle was last dead
        public int _tickControlTime;        //The time at which the vehicle was taken control of
        public int _tickControlEnd;         //How long the duration is

        public int _tickAntiFire;           //The time until fire has been disabled
        public int _tickAntiRotate;         //The time until rotation has been disabled
        public int _tickAntiRecharge;       //The time until energy regen has been disabled
        #endregion

        #region Events
        //public event Action<Vehicle> Destroyed;	//Called when the vehicle has been destroyed
        #endregion

        ///////////////////////////////////////////////////
        // Accessors
        ///////////////////////////////////////////////////

        /// <summary>
        /// Is this player currently dead?
        /// </summary>
        public bool IsDead
        {
            get
            {
                return _state.health == 0 || bCondemned;
            }
        }

        ///////////////////////////////////////////////////
        // Member Classes
        ///////////////////////////////////////////////////
        #region Member Classes

        #endregion

        ///////////////////////////////////////////////////
        // Member Functions
        ///////////////////////////////////////////////////
        /// <summary>
        /// Generic constructor
        /// </summary>
        public Vehicle(VehInfo type, Arena arena)
        {	//Populate variables
            _type = type;
            _arena = arena;

            _childs = new List<Vehicle>();

            _state = new Helpers.ObjectState();

        }

        /// <summary>
        /// Generic constructor
        /// </summary>
        public Vehicle(VehInfo type, Helpers.ObjectState state, Arena arena)
        {	//Populate variables
            _type = type;
            _arena = arena;

            _childs = new List<Vehicle>();

            _state = state;
            _attackers = new List<Player>();

        }

        /// <summary>
        /// Initialize the state with the default health, energy, etc
        /// </summary>
        public void assignDefaultState()
        {
            _state.health = (short)(_type.Hitpoints == 0 ? 1 : _type.Hitpoints);
            _state.energy = (short)_type.EnergyMax;
        }


        #region ILocatable functions
        public ushort getID() { return _id; }
        public Helpers.ObjectState getState() { return _state; }
        #endregion

        #region State
        #endregion

        #region Game State
        #endregion 

        #region Helpers
        #endregion
    }
}