using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace FreeInfantryClient.Windows.ZoneList.Directory
{
    public class Zone
    {
        public string _name;
        public IPEndPoint _endpoint;
        public int port;

        public Zone(string name, string ipaddress, int port)
        {
            IPAddress address = IPAddress.Parse(ipaddress);
            _endpoint = new IPEndPoint(address, port);
            _name = name;
        }
    }
}
