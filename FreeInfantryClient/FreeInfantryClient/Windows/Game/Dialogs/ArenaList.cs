using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FreeInfantryClient.Game;

namespace FreeInfantryClient.Windows.Dialogs
{
    public partial class dialog_ArenaList : Form
    {
        public List<Arena> _arenas;
        private GameClient _game;

        public dialog_ArenaList(List<Arena> arenas, GameClient game)
        {
            _arenas = arenas;
            _game = game;

            InitializeComponent();
        }

        private void ArenaList_Load(object sender, EventArgs e)
        {
            this.CenterToParent();
        }

        public void showArenas()
        {

            listArenas.Invoke((Action)(() =>
            {
                listArenas.Visible = true;
                int index = 1;
                foreach (Arena arena in _arenas)
                {
                    
                    int maxLength = 64;

                    int indent = maxLength - arena._name.Length;

                    listArenas.Items.Add(String.Format("{0}{1}{2}{3}{4}", index, Indent(5), arena._name, Indent(indent), arena._playerCount));
                    index++;

                }
            }));

        }

        public string Indent(int count)
        {
            return "".PadLeft(count);
        }

        private void listArenas_DoubleClick(object sender, EventArgs e)
        {
            int index = Convert.ToInt32(listArenas.SelectedItem.ToString().Substring(0, 2)) - 1;
            Arena arena = _arenas[index];
            _game.joinArena(arena._name);
            this.Close();
        }

        private void dialog_ArenaList_Leave(object sender, EventArgs e)
        {
            this.Focus();
        }

        private void dialog_ArenaList_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }
    }
}
