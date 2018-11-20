using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

using InfServer.Network;
using InfServer.Protocol;
using InfServer.Logic;

using Assets;
using FreeInfantryClient.Settings;


namespace FreeInfantryClient.Game
{
    // Player Class
    /// Represents a single player in the server
    ///////////////////////////////////////////////////////
    public partial class Player : CustomObject
    {


        public void poll()
        {
            CS_PlayerUpdate update = new CS_PlayerUpdate();

            update.positionX = _state.positionX;
            update.positionY = _state.positionY;
            update.positionZ = _state.positionZ;
            update.direction = (ushort)_state.direction;
            update.pitch = _state.pitch;
            update.unk1 = _state.unk1;
            update.yaw = _state.yaw;
            update.velocityX = _state.velocityX;
            update.velocityY = _state.velocityY;
            update.velocityZ = _state.velocityZ;

            update.playerID = _id;
            update.health = _state.health;

            _gameclient._conn._client.send(update);
        }

    }
}