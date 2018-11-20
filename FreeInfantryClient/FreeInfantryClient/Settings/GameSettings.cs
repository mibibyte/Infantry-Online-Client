using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;


namespace FreeInfantryClient.Settings
{
    internal class GameSettings
    {
        private static IniFile settings;
        private static string currentDirectory;




        #region Launcher Settings
        internal class Launcher
        {
            public static string _accounts { get; set; }
            public static string _accountsBackup { get; set; }
            public static string _assets { get; set; }
            public static string _assetsList { get; set; }
            public static string _launcherAssets { get; set; }
            public static string _launcherAssetList { get; set; }
            public static string _directory1 { get; set; }
            public static string _directory2 { get; set; }
            public static string _version { get; set; }
            public static string _versionURL { get; set; }
            public static string _website { get; set; }
            public static string _donate { get; set; }
            public static string _discord { get; set; }
        }
        #endregion

        #region Credentials
        internal class Credentials
        {
            public static string _username { get; set; }
            public static string _password { get; set; }
            public static string _reminder { get; set; }
            public static string _alias { get; set; }
            public static string _passwordLength { get; set; }
        }
        #endregion

        #region Chats
        internal class Chats
        {
            public static List<string> _chats;
        }
        #endregion

        #region Public Calls
        /// <summary>
        /// Initiates our settings
        /// </summary>
        /// <returns>Returns true if successful</returns>
        public static bool Initiate()
        {
            currentDirectory = Directory.GetCurrentDirectory(); //Lets set our working directory
            string settingsIni = (Path.Combine(currentDirectory, "settings.ini"));
            string defaultIni = (Path.Combine(currentDirectory, "default.ini"));
            if (File.Exists(settingsIni))
            {   //Do a quick ini comparison to see if there hasn't been an ini update
                IniCompare();

                settings = new IniFile(settingsIni);
                if (!settings.Load())
                { return false; }

                settings.sections["Launcher"].setting["Version"] = Application.ProductVersion;
                WinRegistry.WriteAddressKeys(settings.Get("Launcher", "Directory1"), settings.Get("Launcher", "Directory2"));
                settings.Save();
                return true;
            }

            if (File.Exists(defaultIni))
            {
                try
                { File.Copy("default.ini", "settings.ini", false); }
                catch (Exception e)
                { MessageBox.Show(e.ToString()); }

                settings = new IniFile(settingsIni);
                if (!settings.Load())
                { return false; }

                settings.sections["Launcher"].setting["Version"] = Application.ProductVersion;
                WinRegistry.WriteAddressKeys(settings.Get("Launcher", "Directory1"), settings.Get("Launcher", "Directory2"));
                settings.Save();
                return true;
            }

            return false;
        }

        public static void Populate()
        {
            #region Launcher Settings
            Launcher._accounts = settings.sections["Launcher"].setting["Accounts"];
            Launcher._accountsBackup = settings.sections["Launcher"].setting["AccountsBackup"];
            Launcher._assets = settings.sections["Launcher"].setting["Assets"];
            Launcher._assetsList = settings.sections["Launcher"].setting["AssetsList"];
            Launcher._launcherAssets = settings.sections["Launcher"].setting["LauncherAssets"];
            Launcher._launcherAssetList = settings.sections["Launcher"].setting["LauncherAssetsList"];
            Launcher._directory1 = settings.sections["Launcher"].setting["Directory1"];
            Launcher._directory2 = settings.sections["Launcher"].setting["Directory2"];
            Launcher._version = settings.sections["Launcher"].setting["Version"];
            Launcher._versionURL = settings.sections["Launcher"].setting["VersionUrl"];
            Launcher._website = settings.sections["Launcher"].setting["Website"];
            Launcher._donate = settings.sections["Launcher"].setting["DonateLink"];
            Launcher._discord = settings.sections["Launcher"].setting["DiscordLink"];
            #endregion

            #region Credential Settings
            Credentials._reminder = settings.sections["Credentials"].setting["Reminder"];
            Credentials._alias = settings.sections["Credentials"].setting["Alias"];
            Credentials._username = settings.sections["Credentials"].setting["Username"];
            Credentials._password = settings.sections["Credentials"].setting["Password"];
            Credentials._passwordLength = settings.sections["Credentials"].setting["PasswordLength"];
            #endregion

            #region Chat Settings
            Chats._chats = new List<string>();
            foreach (string chat in settings.sections["Chat"].GetValues())
            {
                if (string.IsNullOrEmpty(chat))
                    continue;

                Chats._chats.Add(chat);
            }
            #endregion


        }

        /// <summary>
        /// Saves the current state of our gamesettings
        /// </summary>
        public static void Save()
        {
            #region Launcher Settings
            settings.sections["Launcher"].setting["Accounts"] = Launcher._accounts;
            settings.sections["Launcher"].setting["AccountsBackup"] = Launcher._accountsBackup;
            settings.sections["Launcher"].setting["Assets"] = Launcher._assets;
            settings.sections["Launcher"].setting["AssetsList"] = Launcher._assetsList;
            settings.sections["Launcher"].setting["LauncherAssets"] = Launcher._launcherAssets;
            settings.sections["Launcher"].setting["LauncherAssetsList"] = Launcher._launcherAssetList;
            settings.sections["Launcher"].setting["Directory1"] = Launcher._directory1;
            settings.sections["Launcher"].setting["Directory2"] = Launcher._directory2;
            settings.sections["Launcher"].setting["Version"] = Launcher._version;
            settings.sections["Launcher"].setting["VersionUrl"] = Launcher._versionURL;
            settings.sections["Launcher"].setting["Website"] = Launcher._website;
            settings.sections["Launcher"].setting["DonateLink"] = Launcher._donate;
            settings.sections["Launcher"].setting["DiscordLink"] = Launcher._discord;
            #endregion

            #region Credential Settings
            settings.sections["Credentials"].setting["Reminder"] = Credentials._reminder;
            settings.sections["Credentials"].setting["Alias"] = Credentials._alias;
            settings.sections["Credentials"].setting["Username"] = Credentials._username;
            settings.sections["Credentials"].setting["Password"] = Credentials._password;
            settings.sections["Credentials"].setting["PasswordLength"] = Credentials._passwordLength;
            #endregion

            #region Chat Settings
            int chatcount = Chats._chats.Count();
            int index = 0;
            foreach (string chat in Chats._chats)
            {
                settings.sections["Chat"].setting["Channel" + index] = chat;
                index++;

                if (index == 5)
                    break;
            }
            #endregion

            settings.Save();

        }

        #endregion

        #region Private Calls

        /// <summary>
        /// Checks our versions between strings by turning it into an int then checking for equals or greater than
        /// </summary>
        /// <returns>Returns true if it matches</returns>
        private static bool VersionCheck(string currentVersion, string version)
        {
            try
            {
                int versionNumberA = int.Parse(StripChar(currentVersion.Trim(), '.', ' '));
                int versionNumberB = int.Parse(StripChar(version.Trim(), '.', ' '));
                if (versionNumberA >= versionNumberB)
                { return true; }
            }
            catch
            { }
            return false;
        }

        /// <summary>
        /// Parses through each INI file checking for matches and if needed, fixes them
        /// </summary>
        private static void IniCompare()
        {
            return;
        }

        /// <summary>
        /// Strips any character parameter out of a specified string
        /// </summary>
        private static string StripChar(string str, params char[] remove)
        {
            string result = string.Empty;
            bool skip;
            foreach (char s in str)
            {
                skip = false;
                foreach (char c in remove)
                {
                    if (s == c)
                    { skip = true; }
                }
                if (!skip)
                { result += s; }
            }

            return result;
        }

        #endregion
    }
}

