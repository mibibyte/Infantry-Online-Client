using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;
using System.Drawing;

using FreeInfantryClient.Game;
using FreeInfantryClient.Windows.Account.Controllers;
using FreeInfantryClient.Settings;
using FreeInfantryClient.Encryption;

namespace FreeInfantryClient.Windows.Account
{
    public partial class Login : Form
    {

        #region Public Accessible Calls

        /// <summary>
        /// Updates our status message and displays the text
        /// </summary>
        public void UpdateStatusMsg(string status)
        {
            Status.Visible = true;
            Status.Text = status;
        }

        /// <summary>
        /// Disables our status messages
        /// </summary>
        public void DisableStatusMsg()
        {
            Status.Visible = false;
            Status.Text = string.Empty;
        }

        /// <summary>
        /// Updates our progress bar
        /// </summary>
        public void UpdateProgressBar(int totalPercentage)
        {
            ProgressBar.Value = totalPercentage;
        }

        /// <summary>
        /// When set by our patcher.exe, it will bypass downloading due to some error within the patcher
        /// </summary>
        public bool BypassDownload { get; set; }

        #endregion

        #region Setup and Initializes

        public Login()
        {
            InitializeComponent();
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);

            Shown += MainForm_Shown;
            AccountController.ShowReminder += ShowReminder;
            currentDirectory = Directory.GetCurrentDirectory(); //Lets set our working directory
        }

        /// <summary>
        /// Initiates our settings
        /// </summary>
        /// <returns>Returns true if successful</returns>
        public bool Initiate()
        {

            if (!GameSettings.Initiate())
            {
                MessageBoxForm msgBox = new MessageBoxForm();
                msgBox.Write("Error while Initiating Launcher",
                    "Could not find your settings files.\r\nPlease make sure you installed the Infantry Launcher\r\nfrom our website below.");
                msgBox.ShowLinkLabel(GetWebsite());
                msgBox.ShowDialog();
                return false;
            }

            //Populate all of our stored variables
            GameSettings.Populate();

            LoadUserSettings();
            return true;
        }

        /// <summary>
        /// Load is called during initializing but before the layout is started
        /// </summary>
        private void Login_Load(object sender, EventArgs e)
        {
            //Check for possible custom skinning and load it
            string imgs = (Path.Combine(currentDirectory, "imgs"));
            if (Directory.Exists(imgs))
            {
                try
                {
                    BackgroundImage = Image.FromFile(Path.Combine(imgs, "bg.png"), true);
                    imgBtnOff = Image.FromFile(Path.Combine(imgs, "btnoff.png"), true);
                    imgBtnOn = Image.FromFile(Path.Combine(imgs, "btnon.png"), true);
                    PlayButton.Image = imgBtnOff;
                    SignUpButton.Image = imgBtnOff;
                }
                catch //If they still dont exist just continue
                { }
            }
            AcceptButton = PlayButton;
        }

        /// <summary>
        /// Called once the main form is shown to the user for the first time
        /// </summary>
        private void MainForm_Shown(object sender, EventArgs e)
        {
            isDownloading = true;
            Refresh();

            PingServer();

            if (!ServerInactive)
            { CheckForUpdates(); }
        }

        #endregion

        #region Button Events

        private void WebsiteLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(GetWebsite());
        }

        private void DonateLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(GetDonate());
        }

        private void DiscordLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(GetDiscord());
        }

        private void PswdHint_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //When clicked, allows you to change your password reminder
            string reminder = GameSettings.Credentials._reminder;
            using (var form = new ReminderForm(reminder))
            {
                var result = form.ShowDialog();
                if (result == DialogResult.OK)
                {
                    GameSettings.Credentials._reminder = form.Reminder; 
                }
            }
        }

        private void ForgotPswd_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //Server inactive? Don't do anything
            if (ServerInactive)
            { return; }

            if (MissingFile("Newtonsoft.Json.dll"))
            { return; }

            new RecoveryForm().ShowDialog();
        }

        private void SignUpButton_Click(object sender, EventArgs e)
        {
            //If the server is inactive, dont do anything
            if (ServerInactive)
            { return; }

            if (MissingFile("Newtonsoft.Json.dll"))
            { return; }

            if (string.IsNullOrEmpty(UsernameBox.Text) && string.IsNullOrEmpty(PasswordBox.Text))
            { new RegisterForm().ShowDialog(); }
            else
            {   //Lets autofill first
                RegisterForm regForm = new RegisterForm();
                regForm.AutoFill(UsernameBox.Text, PasswordBox.Text);
                regForm.ShowDialog();
            }
        }

        private void PlayButton_Click(object sender, EventArgs e)
        {
            //If the server is inactive, dont do anything
            if (ServerInactive || isDownloading)
            { return; }

            if (string.IsNullOrWhiteSpace(UsernameBox.Text))
            {
                MessageBox.Show("Username cannot be blank.", "Infantry Online");
                return;
            }

            if (string.IsNullOrWhiteSpace(PasswordBox.Text))
            {
                MessageBox.Show("Password cannot be blank.", "Infantry Online");
                return;
            }

            //Try loading first
            string pswd = GameSettings.Credentials._password;

            //Did the text change at all?
            if (pswdTextChanged == true)
            { //Since it did, use this instead
                pswd = Md5.Hash(PasswordBox.Text.Trim());
            }

            switch (RememberPwd.CheckState)
            {
                case CheckState.Checked:
                    //Was the text changed? We don't overwrite till neccessary
                    if (pswdTextChanged == true)
                    { GameSettings.Credentials._password = Md5.Hash(PasswordBox.Text.Trim()); }
                    GameSettings.Credentials._passwordLength = PasswordBox.Text.Length.ToString();
                    GameSettings.Save();
                    break;

                case CheckState.Unchecked:
                    GameSettings.Credentials._password = string.Empty;
                    GameSettings.Credentials._passwordLength = string.Empty;
                    GameSettings.Save();
                    break;
            }

            GameSettings.Credentials._username = UsernameBox.Text.Trim();

            //Get our ticket id
            UpdateStatusMsg("Trying Credentials...");
            Cursor.Current = Cursors.WaitCursor;
            pswdTextChanged = false;

            if (MissingFile("Newtonsoft.Json.dll"))
            { return; }

            string[] response = AccountController.LoginServer(UsernameBox.Text.Trim(), pswd);
            if (response != null)
            {
                //Load our zone list window
                this.Hide();
                frmZoneList zonelist = new frmZoneList(response[0]);
                zonelist.ShowDialog();
                this.Close();
            }

            Cursor.Current = Cursors.Default;
            DisableStatusMsg();
        }

        private void Button_MouseEnter(object sender, EventArgs e)
        {
            if (imgBtnOn != null)
            {
                ((Button)sender).Image = imgBtnOn;
            }
        }

        private void Button_MouseLeave(object sender, EventArgs e)
        {
            if (imgBtnOff != null)
            {
                ((Button)sender).Image = imgBtnOff;
            }
        }

        #endregion

        #region Private Status Functions

        private void PasswordBox_TextChanged(object sender, EventArgs e)
        {
            pswdTextChanged = true;
        }

        private void LoadUserSettings()
        {


            string user = GameSettings.Credentials._username;
            string pswd = GameSettings.Credentials._password;


            UsernameBox.Select(); //Activate it

            if (user.Length <= 0)
            { return; }
            UsernameBox.Text = user;

            if (pswd.Length <= 0)
            { return; }
            string data = GameSettings.Credentials._passwordLength;
            string test = string.Empty;
            int length;
            if (string.IsNullOrEmpty(data) || !int.TryParse(data, out length))
            { return; }

            for (int c = 0; c < length; c++)
            { test += "*"; }
            PasswordBox.Text = test;
            pswdTextChanged = false;

            RememberPwd.Checked = true;
        }

        /// <summary>
        /// Pings the server asking if its running
        /// </summary>
        private void PingServer()
        {
            UpdateStatusMsg("Checking Server Status...");

            string url = GameSettings.Launcher._accounts;
            if (!AccountController.PingServer(url))
            {
                url = GameSettings.Launcher._accountsBackup;

                //First time failed, try backup
                if (!AccountController.PingServer(url))
                {
                    DisableStatusMsg();

                    MessageBox.Show("Server is currently not active.\r\nPlease try again in a few minutes or try reporting it to the admins using the discord link above.");
                    serverInactive = true;
                    if (!ServerLabel.Visible)
                    { ServerLabel.Visible = true; }
                    return;
                }
            }

            //Lets set our active server link
            AccountController.CurrentUrl = url;
            UpdateStatusMsg("Server is active.");
        }

        /// <summary>
        /// Checks for launcher and file updates
        /// </summary>
        private void CheckForUpdates()
        {
            //Check to see if we are bypassing
            if (BypassDownload)
            { //We are, bypass downloading
                UpdateComplete();
                return;
            }

            UpdateStatusMsg("Checking for launcher updates...");
            AssetDownloadController.CurrentDirectory = currentDirectory;
            AssetDownloadController.SetForm(this);
            try
            {
                string version = GameSettings.Launcher._version;
                string versionUrl = new System.Net.WebClient().DownloadString(GameSettings.Launcher._versionURL);
                if (!string.IsNullOrWhiteSpace(versionUrl.Trim()) && !version.Equals(versionUrl.Trim(), StringComparison.Ordinal))
                { //Strings dont match, check for greater than by parsing into int
                    if (VersionCheck(version, versionUrl))
                    { UpdateStatusMsg("No launcher files to update. Skipping..."); }
                    else
                    {
                        AssetDownloadController.OnUpdateLauncher += UpdateLauncher;
                        AssetDownloadController.DownloadAssets(GameSettings.Launcher._launcherAssets, GameSettings.Launcher._launcherAssetList);
                        return;
                    }
                }
                else { UpdateStatusMsg("No launcher files to update. Skipping..."); }
            }
            catch
            { UpdateStatusMsg("Cannot download launcher updates....Skipping."); }

            UpdateFiles();
        }

        /// <summary>
        /// Activates our self updater and/or file downloads after launcher downloads are complete
        /// </summary>
        private void UpdateLauncher(bool skipUpdates)
        {
            if (skipUpdates)
            {
                UpdateStatusMsg("No files to update. Skipping...");

                UpdateFiles();
                return;
            }

            UpdateStatusMsg("Downloads complete, updating launcher...");
            System.Threading.Thread.Sleep(1000);

            if (File.Exists(Path.Combine(currentDirectory, "Patcher.exe")))
            {
                //See if we can even find the link to update
                string Location = GameSettings.Launcher._launcherAssets;
                string List = GameSettings.Launcher._launcherAssetList;
                if (!string.IsNullOrEmpty(Location) && !string.IsNullOrEmpty(List))
                {
                    Process updater = new Process();
                    updater.StartInfo.FileName = "Patcher.exe";
                    updater.StartInfo.Arguments = string.Format("Location:{0} Manifest:{1}", Location, List);
                    updater.Start();

                    Application.Exit();
                }
            }

            UpdateStatusMsg("Cannot self update...Skipping.");

            UpdateFiles();
        }

        /// <summary>
        /// Updates our asset files
        /// </summary>
        private void UpdateFiles()
        {
            UpdateStatusMsg("Checking for file updates...");
            try
            {
                AssetDownloadController.OnUpdateFiles += UpdateComplete;
                AssetDownloadController.DownloadAssets(GameSettings.Launcher._assets, GameSettings.Launcher._assetsList);
            }
            catch
            {
                UpdateStatusMsg("Cannot download file updates....Skipping.");
                UpdateComplete();
            }
        }

        /// <summary>
        /// Activates our play button once all downloads are completed
        /// </summary>
        private void UpdateComplete()
        {
            UpdateStatusMsg("Updating complete...");
            PlayButton.Enabled = true;
            isDownloading = false;

            if (ProgressBar.Value != 100)
            { UpdateProgressBar(100); }

            //Delete any unnecessary files
            if (File.Exists(Path.Combine(currentDirectory, "Patcher.exe")))
            {
                //Set file attributes to normal incase of read-only then delete it
                File.SetAttributes(Path.Combine(currentDirectory, "Patcher.exe"), FileAttributes.Normal);
                File.Delete(Path.Combine(currentDirectory, "Patcher.exe"));
            }
        }

        #endregion

        #region Private Calls

        /// <summary>
        /// Checks our versions between strings by turning it into an int then checking for equals or greater than
        /// </summary>
        /// <returns>Returns true if it matches</returns>
        private bool VersionCheck(string currentVersion, string version)
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

       

        private string GetWebsite()
        {
            string defaultLink = @"http://freeinfantry.com/";
            string website = GameSettings.Launcher._website;
            return !string.IsNullOrWhiteSpace(website) ? website : defaultLink;
        }

        private string GetDonate()
        {
            string defaultLink = @"http://freeinfantry.com/";
            string donate = GameSettings.Launcher._donate;
            return !string.IsNullOrWhiteSpace(donate) ? donate : defaultLink;
        }

        private string GetDiscord()
        {
            string defaultLink = @"https://discord.gg/2avPSyv";
            string discord = GameSettings.Launcher._discord;
            return !string.IsNullOrWhiteSpace(discord) ? discord : defaultLink;
        }

        /// <summary>
        /// Shows the password reminder after a certain amount of invalid passwords
        /// </summary>
        private void ShowReminder(int count)
        {
            PswdHint.Enabled = true;
            PswdHint.Visible = true;

            //Get our reminder
            string reminder = GameSettings.Credentials._reminder;
            if (!string.IsNullOrWhiteSpace(reminder))
            {
                PswdHint.LinkBehavior = LinkBehavior.NeverUnderline;

                //Lets align our button
                int strLen = PswdHint.Text.Length + reminder.Length;
                PswdHint.Width = strLen;
                PswdHint.Location = new Point((Size.Width / 2) - (PswdHint.Width / 4 - PswdHint.Margin.Left), PswdHint.Location.Y);
                PswdHint.Text += reminder;
            }
        }

        /// <summary>
        /// If the server returned no signal, activate our message
        /// </summary>
        private bool ServerInactive
        {
            get
            {
                if (serverInactive)
                {
                    if (!ServerLabel.Visible)
                    { ServerLabel.Visible = true; }
                    return true;
                }
                return false;
            }
        }

        /// <summary>
        /// Strips any character parameter out of a specified string
        /// </summary>
        private string StripChar(string str, params char[] remove)
        {
            string result = string.Empty;
            bool skip;
            foreach(char s in str)
            {
                skip = false;
                foreach(char c in remove)
                {
                    if (s == c)
                    { skip = true; }
                }
                if (!skip)
                { result += s; }
            }

            return result;
        }

        /// <summary>
        /// Determines if there is an important file missing in our directory and reports it
        /// </summary>
        private bool MissingFile(string fileName)
        {
            if (File.Exists(Path.Combine(Environment.CurrentDirectory, fileName)))
            { return false; }

            Cursor.Current = Cursors.Default;

            MessageBox.Show(string.Format("Error: Cannot locate {0}.{1}Try restarting the launcher or using\n\rthe repair button in the installer.", fileName, "\n\r\n\r"), "File Missing");
            DisableStatusMsg();
            return true;
        }

        #endregion
        private string currentDirectory;
        private Image imgBtnOff;
        private Image imgBtnOn;
        private bool serverInactive;
        private bool isDownloading;
        private bool pswdTextChanged;
    }
}