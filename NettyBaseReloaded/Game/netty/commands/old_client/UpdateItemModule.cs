﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands.old_client
{
    class UpdateItemModule
    {
        public const short ID = 577;

        public LabItemModule itemToUpdate;

        public OreCountModule oreCountToUpdateWith;

        public UpdateItemModule(LabItemModule itemToUpdate, OreCountModule oreCountToUpdateWith)
        {
            this.itemToUpdate = itemToUpdate;
            this.oreCountToUpdateWith = oreCountToUpdateWith;
        }

        public byte[] write()
        {
            var cmd = new ByteArray(ID);
            cmd.AddBytes(itemToUpdate.write());
            cmd.AddBytes(oreCountToUpdateWith.write());
            return cmd.Message.ToArray();
        }
    }
}
