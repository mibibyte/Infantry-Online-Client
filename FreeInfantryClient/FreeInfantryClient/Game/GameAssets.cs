using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Threading;
using System.Text;
using System.IO;
using FreeInfantryClient.Windows;
using InfServer.Network;
using InfServer.Protocol;
using InfServer;
using FreeInfantryClient.Game.Commands;
using FreeInfantryClient.Game.Protocol;
using Assets;


namespace FreeInfantryClient.Game
{
    public partial class GameClient : IClient
    {
        public AssetManager _assets;


        public bool loadAssets(string zoneConfig)
        {
            string assetsPath = "assets\\";

            //Load our zone config
            InfServer.Log.write(InfServer.TLog.Normal, "Loading Zone Configuration");

            if (!System.IO.Directory.Exists(assetsPath))
            {
                InfServer.Log.write(InfServer.TLog.Error, "Unable to find assets directory '" + assetsPath + "'.");
                return false;
            }

            string filePath = AssetFileFactory.findAssetFile(zoneConfig, assetsPath);
            if (filePath == null)
            {
                InfServer.Log.write(InfServer.TLog.Error, "Unable to find config file '" + assetsPath + zoneConfig + "'.");
                return false;
            }

            _zoneConfig = CfgInfo.Load(filePath);

            //Load assets from zone config and populate AssMan
            try
            {
                _assets = new AssetManager();

                _assets.bUseBlobs = false;


                if (!_assets.load(_zoneConfig, zoneConfig))
                {	//We're unable to continue
                    InfServer.Log.write(InfServer.TLog.Error, "Files missing, unable to continue.");
                    return false;
                }
            }
            catch (System.IO.FileNotFoundException ex)
            {	//Report and abort
                InfServer.Log.write(InfServer.TLog.Error, "Unable to find file '{0}'", ex.FileName);
                return false;
            }
            return true;
        }
    }
}
