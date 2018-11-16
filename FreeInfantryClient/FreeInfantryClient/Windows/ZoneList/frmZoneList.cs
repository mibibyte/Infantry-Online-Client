using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Net;
using System.Globalization;
using FreeInfantryClient.Game;
using FreeInfantryClient.Windows.ZoneList.Directory;

namespace FreeInfantryClient.Windows
{
    public partial class frmZoneList : Form
    {
        public IPEndPoint _endPoint;
        public string _ticketid;
        public List<Zone> _zones;
        public FreeInfantryClient.Windows.ZoneList.Directory.Directory _directory;

        public frmZoneList(string ticketid)
        {
            _zones = new List<Zone>();
            _directory = new ZoneList.Directory.Directory(this);
            _ticketid = ticketid;

            //Allow functions to pre-register
            InfServer.Logic.Registrar.register();

            InitializeComponent();
        }

        private void frmZoneList_Load(object sender, EventArgs e)
        {
            alias.Text = Settings.GameSettings.Credentials._alias;

            foreach (Zone zone in _zones)
            {
                listZones.Items.Add(zone._name);
            }

        }

        private void listZones_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(alias.Text))
            {
                Settings.GameSettings.Credentials._alias = alias.Text;
            }

            Zone zone = null;
            zone = _zones.FirstOrDefault(z => z._name == listZones.SelectedItem.ToString());

            if (zone != null)
            {
                Game game = new Game(zone._endpoint, _ticketid, alias.Text);
                this.Hide();
                game.ShowDialog();
                this.Show();
            }
            
        }
    }
}
