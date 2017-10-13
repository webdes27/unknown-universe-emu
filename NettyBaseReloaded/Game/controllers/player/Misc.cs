﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.controllers.implementable;
using NettyBaseReloaded.Game.netty;
using NettyBaseReloaded.Game.objects.world;
using NettyBaseReloaded.Game.objects.world.characters.cooldowns;
using NettyBaseReloaded.Game.objects.world.map.objects;
using NettyBaseReloaded.Game.objects.world.players.settings.slotbars;

namespace NettyBaseReloaded.Game.controllers.player
{
    class Misc : IChecker
    {
        // TODO: Make every function return 0 / 1 & stuff to be handled by the response.

        private PlayerController baseController;

        private jClass JClass { get; set; }

        public Misc(PlayerController controller)
        {
            baseController = controller;
            JClass = new jClass(controller);
        }

        public void Check()
        {
            JClass.Checker();
        }

        /// <summary>
        /// Executes the item function depending of the selected one
        /// </summary>
        public void UseItem(string itemId)
        {
            var player = (Player)baseController.Player;

            //Console.WriteLine(itemId);
            if (player.Settings.Slotbar._items.ContainsKey(itemId))
            {
                var item = player.Settings.Slotbar._items[itemId];

                if (item.Visible && (item.Activable || item is RocketItem))
                {
                    //This is the magic function :D
                    item.Execute(player);
                }
            }
        }

        public void ChangeConfig(int targetConfigId = 0)
        {
            if (baseController.Character.Cooldowns.Exists(x => x is ConfigCooldown)) return;

            baseController.Character.Cooldowns.Add(new ConfigCooldown());

            targetConfigId = baseController.Player.CurrentConfig == 2 ? 1 : 2;

            baseController.Player.CurrentConfig = targetConfigId;

            baseController.Player.Update();

            Packet.Builder.LegacyModule(World.StorageManager.GetGameSession(baseController.Player.Id)
                , "0|A|CC|" + baseController.Player.CurrentConfig);
        }

        private class jClass
        {
            private DateTime JumpEndTime = new DateTime(2017, 1, 24, 0, 0, 0);

            private PlayerController baseController;

            private Spacemap TargetMap { get; set; }
            private Vector TargetPosition { get; set; }

            public jClass(PlayerController baseController)
            {
                this.baseController = baseController;
            }

            public void Initiate(int targetMapId, Vector targetPos, int portalId = -1)
            {
                if (baseController.Dead || baseController.StopController) return;

                TargetMap = World.StorageManager.Spacemaps[targetMapId];
                TargetPosition = targetPos;

                var gameSession = World.StorageManager.GetGameSession(baseController.Player.Id);
                if (TargetMap.Level > baseController.Player.Information.Level.Id)
                {
                    Packet.Builder.LegacyModule(gameSession, $"0|k|{TargetMap.Level}");
                    Cancel();
                    return;
                }
                if (portalId != -1)
                {
                    Packet.Builder.ActivatePortalCommand(gameSession, baseController.Player.Spacemap.Objects[portalId] as Jumpgate);
                }
                JumpEndTime = DateTime.Now.AddSeconds(3);
                baseController.Jumping = true;
            }

            public void Checker()
            {
                if (baseController.Jumping) Refresh();
            }

            void Refresh()
            {
                if (baseController.Dead || baseController.StopController)
                {
                    Cancel();
                    return;
                }

                if ((baseController.Attack.Attacked || baseController.Attack.Attacking) && baseController.Player.Spacemap.Pvp)
                {
                    Cancel();
                    return;
                }

                if (DateTime.Now > JumpEndTime)
                {
                    var gameSession = World.StorageManager.GetGameSession(baseController.Player.Id);
                    Packet.Builder.MapChangeCommand(gameSession);
                    baseController.Player.Spacemap = TargetMap;
                    baseController.Player.Position = TargetPosition;
                    baseController.Player.Save();
                    baseController.Destruction.Remove(baseController.Player);
                    Reset();
                    gameSession.Disconnect();
                }
            }

            void Cancel()
            {
                Reset();
            }

            void Reset()
            {
                baseController.Jumping = false;
                JumpEndTime = DateTime.Now;
                TargetMap = null;
                TargetPosition = null;
            }
        }

        public void Jump(int targetMapId, Vector targetPos, int portalId = -1)
        {
            JClass.Initiate(targetMapId, targetPos, portalId);
        }

        public void ChangeDroneFormation(DroneFormation targetFormation)
        {
            //if (
            //    !baseController.CooldownStorage.Finished(
            //        objects.world.storages.playerStorages.CooldownStorage.DRONE_FORMATION_COOLDOWN)) return;

            //var gameSession = World.StorageManager.GetGameSession(baseController.Player.Id);
            //baseController.Player.Formation = targetFormation;
            //gameSession.Client.Send(DroneFormationChangeCommand.write(baseController.Player.Id, (int)targetFormation));
            //baseController.CooldownStorage.Start(gameSession, objects.world.storages.playerStorages.CooldownStorage.DRONE_FORMATION_COOLDOWN);
            //baseController.Player.Update();
        }

        private DateTime LastReloadedTime = new DateTime(2016, 1, 1, 0, 0, 0);
        public void ReloadConfigs()
        {
            
        }
    }
}
