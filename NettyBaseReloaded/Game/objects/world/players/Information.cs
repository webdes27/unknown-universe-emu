﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.managers;
using NettyBaseReloaded.Game.objects.world.characters;
using NettyBaseReloaded.Game.objects.world.players.informations;
using NettyBaseReloaded.Networking;

namespace NettyBaseReloaded.Game.objects.world.players
{
    class Information : PlayerBaseClass
    {
        public BaseInfo Experience { get; set; }

        public BaseInfo Honor { get; set; }

        public BaseInfo Credits { get; set; }

        public BaseInfo Uridium { get; set; }

        public Level Level { get; set; }

        public Title Title { get; set; }

        public Dictionary<string, Ammunition> Ammunitions { get; set; }

        public Premium Premium { get; set; }

        public Cargo Cargo { get; set; }

        public int Vouchers;

        public int GGSpins;

        public int[] BootyKeys;

        public Dictionary<int, int> KilledShips;

        public Information(Player player) : base(player)
        {
            Experience = new Exp(player);
            Honor = new Honor(player);
            Credits = new Credits(player);
            Uridium = new Uridium(player);
            Premium = new Premium();
            Ammunitions = World.DatabaseManager.LoadAmmunition(Player);
            Cargo = World.DatabaseManager.LoadCargo(player);

            UpdateAll();
            Level = World.StorageManager.Levels.DeterminatePlayerLvl(Experience.Get());
            KilledShips = World.DatabaseManager.LoadStats(player);
            World.DatabaseManager.LoadExtraData(player, this);
            player.Ticked += Ticked;
        }

        private void Ticked(object sender, EventArgs eventArgs)
        {
            if (LastUpd.AddSeconds(3) > DateTime.Now) return;

            UpdateAll();
            LastUpd = DateTime.Now;
        }

        public void AddKill(int shipId)
        {
            if (Player.Information.KilledShips.ContainsKey(shipId))
                Player.Information.KilledShips[shipId]++;
            else Player.Information.KilledShips.Add(shipId, 1);
        }

        private DateTime LastUpd = new DateTime();
        
        public void UpdateAll()
        {
            World.DatabaseManager.PerformFullRefresh(this);
        }

        /* THIS IS NOT A INFO SETTER !!!!!!! */
        public void UpdateInfoBulk(double creChange, double uriChange, double expChange, double honChange)
        {
            World.DatabaseManager.UpdateInfoBulk(Player, creChange, uriChange, expChange, honChange);
        }

        public void LevelUp(Level targetLevel)
        {
            Level = targetLevel;
            World.DatabaseManager.SetInfo(Player, "LVL", targetLevel.Id);
        }

        public void UpdateTitle()
        {
            if (Title != null)
            {
                GameClient.SendRangePacket(Player, netty.commands.old_client.LegacyModule.write($"0|n|t|{Player.Id}|{Title.ColorId}|{Title.Key}"), true);
                GameClient.SendRangePacket(Player, netty.commands.new_client.LegacyModule.write($"0|n|t|{Player.Id}|{Title.ColorId}|{Title.Key}"), true);
            }
            else
            {
                GameClient.SendRangePacket(Player, netty.commands.old_client.LegacyModule.write($"0|n|trm|{Player.Id}"), true);
                GameClient.SendRangePacket(Player, netty.commands.new_client.LegacyModule.write($"0|n|trm|{Player.Id}"), true);
            }
        }
    }
}
