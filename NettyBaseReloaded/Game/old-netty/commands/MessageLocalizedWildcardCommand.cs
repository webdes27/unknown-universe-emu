﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands
{
    class MessageLocalizedWildcardCommand
    {
        public const short ID = 19731;

        public string baseKey;
        public List<MessageWildcardReplacementModule> wildCardReplacements;

        public MessageLocalizedWildcardCommand(string baseKey, List<MessageWildcardReplacementModule> wildCardReplacements)
        {
            this.baseKey = baseKey;
            this.wildCardReplacements = wildCardReplacements;
        }

        public byte[] write()
        {
            var cmd = new ByteArray(ID);
            cmd.UTF(baseKey);
            cmd.Integer(wildCardReplacements.Count);
            foreach (var loc in wildCardReplacements)
            {
                cmd.AddBytes(loc.write());
            }
            return cmd.Message.ToArray();
        }
    }
}
