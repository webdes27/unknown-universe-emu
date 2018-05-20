﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NettyBaseReloaded.Game.objects.world.players.quests
{
    class QuestRoot
    {
        public int Id { get; set; }

        public bool Active { get; set; }

        public bool Ordered { get; set; }

        public bool Mandatory { get; set; }
        
        public int MandatoryCount { get; set; }

        public List<QuestElement> Elements { get; set; }

        public QuestRoot()
        {
            Elements = new List<QuestElement>();
        }

        public void LoadPlayerData(Player player)
        {
            if (player == null) return;
            //Element[0]
            //[0, {true, 10, false}]
            //Elements[0].Condition.State.Completed = true;
            //Elements[0].Condition.State.CurrentValue = 10;
            //Elements[0].Condition.State.Active = false;
        }
    }
}
