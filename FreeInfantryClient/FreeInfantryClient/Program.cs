using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Threading;
using FreeInfantryClient.Windows;
using FreeInfantryClient.Game;
using FreeInfantryClient.Game.Protocol;


namespace FreeInfantryClient
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Windows.Login login = new Windows.Login();
                login.BypassDownload = true;

                if (login.Initiate())
                {
                    Application.Run(login);
                }
                else
                {
                    Application.Exit();
                }


            }
        }
        private static void ConsoleMain()
        {

        }

        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        private static extern bool AllocConsole();
    }
}
