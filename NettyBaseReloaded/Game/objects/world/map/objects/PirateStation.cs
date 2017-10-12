﻿using System.Collections.Generic;

namespace NettyBaseReloaded.Game.objects.world.map.objects
{
    class PirateStation : Station
    {
        public PirateStation(int id, Vector pos) : base(id, new List<StationModule>(), Faction.NONE, pos)
        {

        }
    }
}