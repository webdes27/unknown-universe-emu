﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands
{
    class ShipWarpCompletedCommand
    {
        public const short ID = 5630;

        public static byte[] write()
        {
            var cmd = new ByteArray(ID);
            return cmd.ToByteArray();
        }
    }
}
