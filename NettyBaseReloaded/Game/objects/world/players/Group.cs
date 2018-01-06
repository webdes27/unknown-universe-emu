﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using NettyBaseReloaded.Game.controllers;
using NettyBaseReloaded.Game.netty;

namespace NettyBaseReloaded.Game.objects.world.players
{
    class Group
    {
        public const int LOOT_MODE_RANDOM = 1;
      
        public const int LOOT_MODE_NEED_BEFORE_GREED = 2;

        /// <summary>
        /// What You Think Is What You Get
        /// </summary>
        public const int LOOT_MODE_WYTIWYG = 3;

        public const int DEFAULT_MAX_GROUP_SIZE = 7;

        public int Id { get; }
        /// <summary>
        /// Group leader
        /// </summary>
        public Player Leader { get; set; }

        /// <summary>
        /// Group members
        /// </summary>
        public Dictionary<int, Player> Members = new Dictionary<int, Player>();

        public bool LeaderInvitesOnly { get; set; }

        public int LootMode { get; set; }

        public Group(Player player, Player acceptedPlayer)
        {
            Id = FindId();
            LootMode = 2;

            Leader = player;
            Leader.Group = this;

            AddToGroup(acceptedPlayer);
            AddToGroup(player);

            DeleteInvitation(player, acceptedPlayer);

            SendInitToAll();
        }

        private int FindId()
        {
            World.StorageManager.Groups.Add(this);
            var id = World.StorageManager.Groups.FindIndex(x => x == this);
            return id;
        }

        private void AddToGroup(Player player)
        {
            Members.Add(player.Id, player);
            player.Group = this;
        }

        public void Tick()
        {
            Update();
        }

        public void SendInitToAll()
        {
            foreach (var player in Members)
            {
                if (player.Value.GetGameSession() == null) continue;
                Packet.Builder.GroupInitializationCommand(player.Value.GetGameSession());
            }
        }

        private DateTime LastUpdate = new DateTime();
        public void Update()
        {
            if (LastUpdate.AddSeconds(1) < DateTime.Now)
            {
                foreach (var groupMemberInstance in Members.Values)
                {
                    var instance = groupMemberInstance.GetGameSession();
                    if (!instance.Player.Controller.Active && !instance.InProcessOfReconection && !instance.InProcessOfDisconnection)
                    {
                        Leave(instance.Player);
                        continue;
                    }
                    if (instance.Player.Group == null)
                    {
                        instance.Player.Group = this;
                        SendInitToAll(); // TEMP FIX
                    }
                    foreach (var _member in Members.Values)
                    {
                        if (_member == null) continue;
                        
                        Packet.Builder.GroupUpdateCommand(instance, _member, GetStats(_member));
                    }
                }
                LastUpdate = DateTime.Now;
            }
        }

        private XElement GetStats(Player player)
        {
            return new XElement("stats",
                new XAttribute("hp", player.CurrentHealth),
                new XAttribute("hpM", player.MaxHealth),
                new XAttribute("nh", player.CurrentNanoHull),
                new XAttribute("nhM", player.MaxNanoHull),
                new XAttribute("sh", player.CurrentShield),
                new XAttribute("shM", player.MaxShield),
                new XAttribute("pos", $"{player.Position.X},{player.Position.Y}"),
                new XAttribute("map", player.Spacemap.Id),
                new XAttribute("lev", player.Information.Level.Id),
                new XAttribute("fra", (int)player.FactionId),
                new XAttribute("act", Convert.ToInt32(true)), // active
                new XAttribute("clk", Convert.ToInt32(player.Controller.Invisible)),
                new XAttribute("shp", Convert.ToInt32(player.Hangar.Ship.Id)),
                new XAttribute("fgt", Convert.ToInt32(player.Controller.Attack.Attacking || player.LastCombatTime.AddSeconds(3) > DateTime.Now)),
                new XAttribute("lgo", Convert.ToInt32(!player.Controller.Active || player.Controller.StopController)),
                new XAttribute("tgt", (player.Selected?.Id).ToString()));
        }

        public void Ping(Vector position)
        {
            foreach (var member in Members)
            {
                if (member.Value.GetGameSession() != null)
                {
                    Packet.Builder.LegacyModule(member.Value.GetGameSession(), $"0|ps|png|{position.X}|{position.Y}");
                }
            }
        }

        public void Follow(Player player, Player followedPlayer)
        {
            var targetPosition = MovementController.ActualPosition(followedPlayer);
            Packet.Builder.HeroMoveCommand(player.GetGameSession(), targetPosition);
            MovementController.Move(player, targetPosition);
        }

        public void Accept(Player inviter, Player acceptedPlayer)
        {
            AddToGroup(acceptedPlayer);
            DeleteInvitation(inviter, acceptedPlayer);
            SendInitToAll();
        }

        public void DeleteInvitation(Player inviter, Player player)
        {
            Packet.Builder.GroupDeleteInvitationCommand(inviter.GetGameSession(), player);
            Packet.Builder.GroupDeleteInvitationCommand(player.GetGameSession(), inviter);
        }

        public void Kick(Player player)
        {
            if (player == Leader)
                return;

            Leave(player);
        }

        public void Destroy()
        {
            World.StorageManager.Groups.Remove(this);
            foreach (var member in Members)
            {
                member.Value.Group = null;
                if (member.Value.GetGameSession() == null) continue;
                Packet.Builder.LegacyModule(member.Value.GetGameSession(), "0|ps|end");
            }
        }

        public void ChangeLeader(GameSession gameSession, GameSession leaderSession)
        {
            if (Leader == leaderSession.Player)
                return;

            Leader = leaderSession.Player;
            foreach (var member in Members)
            {
                if (member.Value.GetGameSession() == null) continue;
                Packet.Builder.LegacyModule(member.Value.GetGameSession(), "0|ps|nl|" + Leader.Id);
            }
        }

        public void ChangeBehavior(GameSession gameSession, int newBehaviour)
        {
            if (gameSession.Player != Leader) return;
            LeaderInvitesOnly = Convert.ToBoolean(newBehaviour);
            foreach (var member in Members)
            {
                if (member.Value.GetGameSession() == null) continue;
                Packet.Builder.LegacyModule(member.Value.GetGameSession(), "0|ps|chib|" + Convert.ToInt32(LeaderInvitesOnly));
            }
        }

        public void Leave(Player player)
        {
            foreach (var member in Members)
            {
                if (member.Value.GetGameSession() == null) continue;
                Packet.Builder.LegacyModule(member.Value.GetGameSession(), "0|ps|lp|lv|" + player.Id);
            }

            player.Group = null;
            Members.Remove(player.Id);
            if (Members.Count > 1)
            {
                if (Leader == player)
                    Leader = Members.FirstOrDefault().Value;

                if (Members.ContainsKey(player.Id))
                {
                    Members.Remove(player.Id);
                }
                SendInitToAll();
            }
            else Destroy();
        }
    }
}