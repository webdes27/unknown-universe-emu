﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloadedController.Utils;

namespace NettyBaseReloadedController.Main.netty.commands
{
    class ChatRequest
    {
        public const short ID = 7;
        public static byte[] write(string text)
        {
            var cmd = new ByteArray(ID);
            cmd.UTF(text);
            return cmd.ToByteArray();
        }
    }
}
