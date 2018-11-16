using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeInfantryClient.Windows.ZoneList.Directory
{
    public class Directory
    {
        private frmZoneList _frmZoneList;

        public Directory(frmZoneList zonelist)
        {
            _frmZoneList = zonelist;


            _frmZoneList._zones.Add(new Zone("[I:League] USL Test Zone", "108.61.133.122", 8026));
            _frmZoneList._zones.Add(new Zone("[I:League] USL KR", "108.61.133.122", 8024));
            _frmZoneList._zones.Add(new Zone("[I:League] USL Isctos", "108.61.133.122", 7022));
            _frmZoneList._zones.Add(new Zone("[I:League] USL EC Apollo", "108.61.133.122", 8124));
            _frmZoneList._zones.Add(new Zone("[I: League] USL Apollo Map","108.61.133.122",7202));
            _frmZoneList._zones.Add(new Zone("[I:Arcade] Zombie Zone", "108.61.133.122", 1012));
            _frmZoneList._zones.Add(new Zone("Localhost [port=2626]", "127.0.0.1", 2626));
        }
    }
}
