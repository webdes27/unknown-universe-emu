﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.netty;

namespace NettyBaseReloaded.Game.objects.world.characters.cooldowns
{
    class BattleRepairRobotCooldown : Cooldown
    {
        public BattleRepairRobotCooldown() : base(DateTime.Now, DateTime.Now.AddSeconds(120))
        {
        }

        public override void OnStart(Character character)
        {
        }

        public override void OnFinish(Character character)
        {
        }

        public override void Send(GameSession gameSession)
        {
            Packet.Builder.LegacyModule(gameSession, "0|A|CLD|BRB|120");
            //TODO: do for new client too
        }
    }
}