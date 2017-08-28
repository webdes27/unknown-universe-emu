﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands.old_client
{
    class ShipRemoveCommand
    {
        public const short ID = 29006;

        public static Command write(int userId)
        {
            var cmd = new ByteArray(ID);
            cmd.Integer(userId);
            return new Command(cmd.ToByteArray(), false);
        }
    }
}
