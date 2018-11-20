using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using System.Windows.Forms;
using System.Net;
using FreeInfantryClient.Game;
using InfServer.Protocol;

namespace FreeInfantryClient.Windows
{
    //To keep this partial class from loading as a designer, kinda hacky, but it works.
    [System.ComponentModel.DesignerCategory("code")]
    public partial class Dummy { }

    public partial class Game
    {


        private void chatLog_Enter(object sender, EventArgs e)
        {
            chatSend.Focus();
        }

        bool isDrag = false;
        int lastY = 0;
        private void chatLog_MouseDown(object sender, MouseEventArgs e)
        {

            //Just add 5px padding
            if (e.Y >= (chatLog.ClientRectangle.Top - 5) &&
                e.Y <= (chatLog.ClientRectangle.Top + 5))
            {
                isDrag = true;
                lastY = e.Y;
            }
        }

        private void chatLog_MouseMove(object sender, MouseEventArgs e)
        {

            if (isDrag)
            {
                chatLog.Height -= (e.Y - lastY);
                chatLog.Top += (e.Y - lastY);
                lastY = e.Y;
            }
        }

        private void chatLog_MouseUp(object sender, MouseEventArgs e)
        {
            if (isDrag)
            {
                isDrag = false;
                chatLog.SelectionColor = Color.Aqua;
            }
        }
    }
}
