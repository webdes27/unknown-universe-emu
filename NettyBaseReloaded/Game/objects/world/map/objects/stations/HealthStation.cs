﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.netty;

namespace NettyBaseReloaded.Game.objects.world.map.objects.stations
{
    class HealthStation : Station
    {
        public List<Player> PlayersInRangeOfStation = new List<Player>();

        public HealthStation(int id, Vector pos) : base(id, new List<StationModule>(), Faction.NONE, pos)
        {
        }

        public override void Tick()
        {
            HealPlayersInRangeOfStation();
        }

        public override void execute(Character character)
        {
            var player = character as Player;
            if (player != null && !PlayersInRangeOfStation.Contains(player))
            {
                PlayersInRangeOfStation.Add(player);
            }
        }

        private void HealPlayersInRangeOfStation()
        {
            foreach (var player in PlayersInRangeOfStation)
            {
                if (player.CurrentHealth != player.MaxHealth && player.LastCombatTime.AddSeconds(10) <= DateTime.Now)
                {
                    var heal = player.MaxHealth / 10;
                    if (player.CurrentHealth + heal > player.MaxHealth) heal = player.MaxHealth - player.CurrentHealth;
                    player.Controller.Heal.Execute(heal, Id);
                    Packet.Builder.LegacyModule(player.GetGameSession(), "0|CSS|1");
                }
                else Packet.Builder.LegacyModule(player.GetGameSession(), "0|CSS|0");
            }
        }
    }
}
