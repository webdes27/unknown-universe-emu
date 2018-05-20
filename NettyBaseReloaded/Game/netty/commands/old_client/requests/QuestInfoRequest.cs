﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands.old_client.requests
{
    class QuestInfoRequest
    {
        public const short ID = 28545;

        public int questId;

        public void readCommand(byte[] bytes)
        {
            var parser = new ByteParser(bytes);
            questId = parser.readInt();
        }
    }
}
