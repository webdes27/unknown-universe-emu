﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands.old_client
{
    class SpaceBallUpdateSpeedCommand
    {
        public const short ID = 23110;

        public static Command write(int factionId, int speed)
        {
            var cmd = new ByteArray(ID);
            cmd.Integer(factionId);
            cmd.Integer(speed);
            return new Command(cmd.ToByteArray(),false);
        }
    }
}