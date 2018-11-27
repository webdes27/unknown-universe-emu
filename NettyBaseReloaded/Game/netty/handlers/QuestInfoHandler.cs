﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.netty.commands.old_client.requests;
using NettyBaseReloaded.Game.objects;
using NettyBaseReloaded.Game.objects.world.players;

namespace NettyBaseReloaded.Game.netty.handlers
{
    class QuestInfoHandler : IHandler
    {
        public void execute(GameSession gameSession, byte[] bytes)
        {
            if (gameSession.Player.UsingNewClient) return;

            var questInfoRequest = new QuestInfoRequest();
            questInfoRequest.readCommand(bytes);

            Packet.Builder.QuestInfoCommand(gameSession, World.StorageManager.Quests[questInfoRequest.questId]);
        }
    }
}
