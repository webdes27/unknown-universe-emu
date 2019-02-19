﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.netty;

namespace NettyBaseReloaded.Game.objects.world.map.objects.jumpgates
{
    class PirateGate : Jumpgate
    {
        public bool Broken;
        public PirateGate(int id, Faction faction, Vector pos, Spacemap map, int destinationMapId, Vector destinationPos, bool isBroken) : base(id, faction, pos, map, destinationPos, destinationMapId, true, 1, 0, 51)
        {
            Broken = isBroken;
            if (isBroken) Gfx = 52;
        }

        public override void click(Character character)
        {
            if (Broken) return;
            base.click(character);
        }
    }
}
