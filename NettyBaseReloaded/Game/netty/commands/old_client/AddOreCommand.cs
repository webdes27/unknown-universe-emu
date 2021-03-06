﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands.old_client
{
    class AddOreCommand
    {
        public const short ID = 24469;

        public static Command write(string hash, OreTypeModule oreType, int x, int y)
        {
            var cmd = new ByteArray(ID);
            cmd.UTF(hash);
            cmd.AddBytes(oreType.write());
            cmd.Integer(x);
            cmd.Integer(y);
            return new Command(cmd.ToByteArray(), false);
        }
    }
}
