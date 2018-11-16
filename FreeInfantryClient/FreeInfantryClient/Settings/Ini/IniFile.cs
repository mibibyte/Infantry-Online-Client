using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace FreeInfantryClient.Settings
{
    public class IniFile
    {
        public Dictionary<string, IniSection> sections;

        /// <summary>
        /// File name of our ini file
        /// </summary>
        public string FileName
        {
            get; private set;
        }

        /// <summary>
        /// Gets a specific value from our ini file, returns empty if not found
        /// </summary>
        public string Get(string element, string section)
        {
            if(HasElement(element) && HasSection(element, section))
            { return sections[element].setting[section]; }

            MessageBox.Show(string.Format("Error: Missing '{0} {1}' in your .ini file.", element, section));
            return string.Empty;
        }

        /// <summary>
        /// Main constructor
        /// </summary>
        public IniFile(string fileName)
        {
            sections = new Dictionary<string, IniSection>();
            FileName = fileName;
        }

        /// <summary>
        /// Adds a line within our .ini file
        /// </summary>
        public string Add(string line)
        {
            if (string.IsNullOrEmpty(line))
                throw new Exception("Ini file must start with a section.");
            if (line.StartsWith("["))
                line = line.TrimStart('[');
            if (line.EndsWith("]"))
                line = line.TrimEnd(']');
            sections.Add(line, new IniSection());
            return line;
        }

        /// <summary>
        /// Loads our .ini file
        /// </summary>
        public bool Load()
        {
            try
            {
                StreamReader streamReader = new StreamReader(FileName);
                string index = "";
                while (streamReader.Peek() != -1)
                {
                    string line = streamReader.ReadLine();
                    if (line.StartsWith("[") && line.EndsWith("]"))
                    {
                        index = Add(line);
                    }
                    else
                    {
                        if (index.Length == 0)
                            throw new Exception("Ini file must start with a section.");
                        sections[index].Add(line);
                    }
                }
                streamReader.Close();
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
                return false;
            }
        }

        /// <summary>
        /// Saves our .ini file
        /// </summary>
        /// <returns>Returns true if saved completed</returns>
        public bool Save()
        {
            try
            {
                StreamWriter streamWriter = new StreamWriter(FileName);
                foreach (string index1 in sections.Keys)
                {
                    streamWriter.WriteLine("[" + index1 + "]");
                    foreach (string index2 in sections[index1].setting.Keys)
                        streamWriter.WriteLine(index2 + "=" + sections[index1].setting[index2]);
                    streamWriter.WriteLine();
                    streamWriter.Flush();
                }
                streamWriter.Close();
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
                return false;
            }
        }

        /// <summary>
        /// Gets a list of each element within our .ini file
        /// </summary>
        public string[] GetElements()
        {
            string[] strArray = new string[sections.Count];
            byte num = 0;
            foreach (KeyValuePair<string, IniSection> keyValuePair in sections)
            {
                strArray[num] = keyValuePair.Key;
                ++num;
            }
            return strArray;
        }

        /// <summary>
        /// Do we have a specific element in the file?
        /// </summary>
        public bool HasElement(string section)
        {
            foreach (KeyValuePair<string, IniSection> keyValuePair in sections)
            {
                if (keyValuePair.Key == section)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Gets all section names under our given element
        /// </summary>
        public string[] GetSections(string element)
        {
            string[] strArray = null;

            if (HasElement(element))
            { strArray = sections[element].GetKeys(); }

            return strArray;
        }

        /// <summary>
        /// Do we have a specific section in the given element?
        /// </summary>
        public bool HasSection(string element, string section)
        {
            if (HasElement(element) && sections[element].setting.ContainsKey(section))
            { return true; }

            return false;
        }
    }
}