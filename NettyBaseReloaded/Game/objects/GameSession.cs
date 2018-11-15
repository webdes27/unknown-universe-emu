﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.controllers;
using NettyBaseReloaded.Game.netty;
using NettyBaseReloaded.Game.objects.world;
using NettyBaseReloaded.Main;
using NettyBaseReloaded.Main.global_managers;
using NettyBaseReloaded.Main.interfaces;
using NettyBaseReloaded.Networking;

namespace NettyBaseReloaded.Game.objects
{
    class GameSession : ITick
    {
        private int TickId { get; set; }
        
        public enum DisconnectionType
        {
            NORMAL,
            INACTIVITY,
            ADMIN,
            SOCKET_CLOSED,
            ERROR
        }

        public Player Player { get; set; }

        public GameClient Client { get; set; }

        public DateTime LastActiveTime = new DateTime(2016, 12, 15, 0, 0, 0);

        public bool InProcessOfReconection = false;

        public bool InProcessOfDisconnection = false;

        public DateTime EstDisconnectionTime = new DateTime();

        public GameSession(Player player)
        {
            Player = player;
            var tickId = -1;
            Global.TickManager.Add(this, out tickId);
            TickId = tickId;
            LastActiveTime = DateTime.Now;
        }

        public int GetId()
        {
            return TickId;
        }

        public void Tick()
        {
            if (LastActiveTime >= DateTime.Now.AddMinutes(5))
                Disconnect(DisconnectionType.INACTIVITY);
            if (EstDisconnectionTime < DateTime.Now && InProcessOfDisconnection)
            {
                //Disconnect(DisconnectionType.NORMAL);
            }
        }

        public static Dictionary<int, GameSession> GetRangeSessions(IAttackable attackable)
        {
            Dictionary<int, GameSession> playerSessions = new Dictionary<int, GameSession>();
            foreach (var player in attackable.Spacemap.Entities.Where(x => x.Value is Player).ToDictionary(x => x.Key, y => y.Value as Player))
            {
                var session = player.Value.GetGameSession();
                playerSessions.Add(player.Key, session);
            }
            return playerSessions;
        }

        public void Relog(Spacemap spacemap = null, Vector pos = null)
        {
            InProcessOfReconection = true;
            PrepareForDisconnect(); // preparation
            spacemap = spacemap ?? Player.Spacemap;
            pos = pos ?? Player.Position;
            Player.Spacemap = spacemap;
            Player.ChangePosition(pos);
            Disconnect(); // closing the socket
            //Player.Save();
        }

        private void PrepareForDisconnect()
        {
            Global.TickManager.Remove(this);
            Global.TickManager.Remove(Player);
            Player.Save();
            Player.Controller.Exit();
            Player.Controller.Destruction.Remove();
        }

        public void Kick()
        {
            PrepareForDisconnect();
            Disconnect();
        }

        /// <summary>
        /// No preparations, just close the socket
        /// </summary>
        public void Disconnect()
        {
            Client.Disconnect();
        }

        public void Disconnect(DisconnectionType dcType)
        {
            if (Player.Pet != null)
            {
                Player.Pet.Controller.Deactivate();
            }
            InProcessOfDisconnection = true;
            if (dcType == DisconnectionType.SOCKET_CLOSED)
            {
                EstDisconnectionTime = DateTime.Now.AddSeconds(30);
                return;
            }
            PrepareForDisconnect();
            Packet.Builder.LegacyModule(this, "ERR|2");
            Client.Disconnect();
            World.StorageManager.GameSessions.Remove(Player.Id);
            InProcessOfDisconnection = false;
            GC.Collect();
        }
    }
}
