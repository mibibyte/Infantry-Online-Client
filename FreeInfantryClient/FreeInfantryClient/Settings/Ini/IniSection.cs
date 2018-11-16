using System;
using System.Collections.Generic;

namespace FreeInfantryClient.Settings
{
    public class IniSection
    {
        public Dictionary<string, string> setting;

        public IniSection()
        {
            setting = new Dictionary<string, string>();
        }
        /// <summary>
        /// Auto adds a key within our section
        /// </summary>
        public void Add(string line)
        {
            if (line.Length == 0)
                return;
            int length = line.IndexOf('=');
            if (length == -1)
                throw new Exception("Keys must have an equal sign.");
            setting.Add(line.Substring(0, length), line.Substring(length + 1, line.Length - length - 1));
        }

        /// <summary>
        /// Returns a string representation of our current key
        /// </summary>
        public string ToString(string key)
        {
            return key + "=" + setting[key];
        }

        /// <summary>
        /// Gets all keys within our section
        /// </summary>
        public string[] GetKeys()
        {
            string[] strArray = new string[setting.Count];
            byte num = 0;
            foreach (KeyValuePair<string, string> keyValuePair in setting)
            {
                strArray[num] = keyValuePair.Key;
                ++num;
            }
            return strArray;
        }

        /// <summary>
        /// Does the current section have this key?
        /// </summary>
        public bool HasKey(string key)
        {
            foreach (KeyValuePair<string, string> keyValuePair in setting)
            {
                if (keyValuePair.Key == key)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Gets all string values in our Ini Section
        /// </summary>
        public string[] GetValues()
        {
            string[] strArray = new string[setting.Count];
            byte num = 0;
            foreach(KeyValuePair<string, string> keyValue in setting)
            {
                strArray[num] = keyValue.Value;
                ++num;
            }
            return strArray;
        }
    }
}