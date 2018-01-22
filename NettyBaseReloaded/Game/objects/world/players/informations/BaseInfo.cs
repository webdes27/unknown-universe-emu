﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NettyBaseReloaded.Game.objects.world.players.informations
{
    abstract class BaseInfo : PlayerBaseClass
    {
        internal double Value { get; set; }

        internal double SyncedValue { get; set; }

        public DateTime LastTimeSynced { get; set; }

        public string SqlName { get; set; }

        public BaseInfo(Player player) : base(player)
        {
            LastTimeSynced = DateTime.Now;
        }

        public abstract void Refresh();

        public abstract void Add(double amount);

        public abstract void Remove(double amount);

        public abstract void Set(double value);

        public double Get()
        {
            return Value;
        }
    }
}
