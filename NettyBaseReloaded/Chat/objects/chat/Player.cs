﻿using NettyBaseReloaded.Chat.controllers;
using NettyBaseReloaded.Game.objects.world.players;
using NettyBaseReloaded.Main.objects;

namespace NettyBaseReloaded.Chat.objects.chat
{
    class Player : Character
    {
        /// <summary>
        /// Player controller
        /// </summary>
        public new PlayerController Controller { get; set; }

        public Player(int id, string name, string sessionId, Clan clan) : base(id, name, sessionId, clan)
        {

        }
    }
}