using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using InfServer.Protocol;

namespace FreeInfantryClient.Game
{
    /// <summary>
    /// Implements a spatial lookup data structure for players in an arena
    /// </summary>
    public class ObjTracker<T> : IEnumerable<T>
        where T : ILocatable
    {
        // These were set based on infantry values
        private const int TICK_MAX = Int16.MaxValue;
        private const int EXACT_TICKS = 16;
        private const int COORD_TICKS = EXACT_TICKS * 80; // 1280

        // Each bucket will be half a coord
        private const int BUCKET_TICKS = COORD_TICKS / 2; // 640
        private const int BUCKET_COUNT = TICK_MAX / BUCKET_TICKS + 1; // 52

        // Use a dictionary to easily look up the last bucket a player was in
        private Dictionary<T, List<T>> _objToBucket;
        private List<T>[,] _matrix;

        private Dictionary<ushort, T> _idToObj;

        // Our default predicate for retrieving objects
        private Func<T, bool> _defPredicate;

        /// <summary>
        /// Generic Constructor
        /// </summary>
        public ObjTracker()
        {
            _defPredicate = null;

            resetStructures();
        }

        /// <summary>
        /// Generic Constructor
        /// </summary>
        public ObjTracker(Func<T, bool> defaultPredicate)
        {
            _defPredicate = defaultPredicate;

            resetStructures();
        }

        /// <summary>
        /// Initializes data structures. Called by the constructor and Clear()
        /// </summary>
        private void resetStructures()
        {
            _objToBucket = new Dictionary<T, List<T>>();

            _idToObj = new Dictionary<ushort, T>();

            // Initialize buckets
            _matrix = new List<T>[BUCKET_COUNT, BUCKET_COUNT];

            for (int i = 0; i < BUCKET_COUNT; i++)
            {
                for (int j = 0; j < BUCKET_COUNT; j++)
                {
                    _matrix[i, j] = new List<T>();
                }
            }
        }

        public void updateObjState(T from, Helpers.ObjectState state)
        {	//Make sure he's one of ours
            try
            {
                if (!Contains(from))
                {
                    InfServer.Log.write(InfServer.TLog.Warning, "Given object state update for unknown object {0}.", from);
                    return;
                }
            }
            catch (Exception e)
            {
                InfServer.Log.write(InfServer.TLog.Warning, String.Format("{0} Details = {1}, {2}", e.ToString(), _idToObj.Count(), from.getID()));
            }

            // Update the bucket if it's not correct
            List<T> newBucket = _matrix[state.positionX / BUCKET_TICKS, state.positionY / BUCKET_TICKS];
            List<T> oldBucket;

            if (!_objToBucket.TryGetValue(from, out oldBucket))
                _objToBucket[from] = newBucket;
            else if (oldBucket != newBucket)
            {	// Move buckets
                oldBucket.Remove(from);
                newBucket.Add(from);

                _objToBucket[from] = newBucket;
            }
        }

        public T getObjByID(ushort id)
        {
            T p = default(T);
            if (!_idToObj.TryGetValue(id, out p))
                return default(T);
            return p;
        }


        #region Collection functions

        public int Count
        {
            get { return _idToObj.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }


        public void Clear()
        {
            resetStructures();
        }

        public bool Contains(T p)
        {
            try
            {
                return _idToObj.ContainsValue(p);
            }
            catch (Exception e)
            {
                InfServer.Log.write(InfServer.TLog.Exception, e.ToString());
                return false;
            }
        }

        public void CopyTo(T[] array, int index)
        {
            try
            {
                _idToObj.Values.CopyTo(array, index);
            }
            catch (Exception e)
            {
                InfServer.Log.write(InfServer.TLog.Warning, String.Format("{0}, (Array Length = {1}, Index = {2})", e.ToString(), array.Count(), index));
                string ushorts = "", values = "";
                ushorts = String.Join(",", _idToObj.Keys);
                values = String.Join(",", _idToObj.Values);
                InfServer.Log.write(InfServer.TLog.Warning, ushorts);
                InfServer.Log.write(InfServer.TLog.Warning, values);
            }
        }

        public bool Remove(T p)
        {
            if (!_objToBucket.ContainsKey(p)) return false;
            else
            {
                // Remove player from ID map
                _idToObj.Remove(p.getID());

                // Take player out of the spatial mapper
                List<T> lastBucket = _objToBucket[p];
                lastBucket.Remove(p);
                _objToBucket.Remove(p);
                return true;
            }
        }
        #endregion

        #region IEnumerable
        /// <summary>
        /// Be careful while using these. Trying to iterate though the player list is subject to change.
        /// Probably not threadsafe when used with the routing code
        /// </summary>		
        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return _idToObj.Values.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _idToObj.Values.GetEnumerator();
        }
        #endregion
    }
}