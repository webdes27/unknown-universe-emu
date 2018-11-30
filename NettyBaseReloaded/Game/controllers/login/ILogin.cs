﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.netty;
using NettyBaseReloaded.Game.netty.commands.old_client;
using NettyBaseReloaded.Game.objects;
using NettyBaseReloaded.Game.objects.world;
using NettyBaseReloaded.Game.objects.world.map;
using NettyBaseReloaded.Game.objects.world.map.collectables;
using NettyBaseReloaded.Game.objects.world.map.objects.assets;
using NettyBaseReloaded.Game.objects.world.map.objects.stations;
using NettyBaseReloaded.Game.objects.world.map.pois;
using NettyBaseReloaded.Game.objects.world.players.extra.techs;
using NettyBaseReloaded.Game.objects.world.players.informations;
using Types = NettyBaseReloaded.Game.objects.world.map.pois.Types;

namespace NettyBaseReloaded.Game.controllers.login
{
    abstract class ILogin
    {
        public GameSession GameSession;

        protected ILogin(GameSession gameSession)
        {
            GameSession = gameSession;
        }

        /// <summary>
        /// Executable for login
        /// </summary>
        public abstract void Execute();

        public void SendSettings()
        {
            Packet.Builder.HotkeysCommand(GameSession);
            Packet.Builder.UserSettingsCommand(GameSession);
            Packet.Builder.SendUserSettings(GameSession);
        }

        public void SendLegacy()
        {
            SendLegacy(GameSession);
            SendCooldowns(GameSession);
        }

        private void SendCooldowns(GameSession gameSession)
        {
            foreach (var cooldown in gameSession.Player.Cooldowns.Cooldowns)
            {
                cooldown.Send(gameSession);
            }
        }

        public static void SendLegacy(GameSession GameSession)
        {
            try
            {
                Packet.Builder.DronesCommand(GameSession, GameSession.Player);
                //Packet.Builder.LegacyModule(GameSession, "0|n|t|" + GameSession.Player.Id + "|222|most_wanted");

                Packet.Builder.LegacyModule(GameSession, "0|A|BK|" + GameSession.Player.Information.BootyKeys[0]); //green booty
                Packet.Builder.LegacyModule(GameSession, "0|A|BKR|" + GameSession.Player.Information.BootyKeys[1]); //red booty
                Packet.Builder.LegacyModule(GameSession, "0|A|BKB|" + GameSession.Player.Information.BootyKeys[2]); //blue booty
                Packet.Builder.LegacyModule(GameSession, "0|A|CC|" + GameSession.Player.CurrentConfig); // Config
                GameSession.Player.LoadExtras();
               
                Packet.Builder.PetInitializationCommand(GameSession, GameSession.Player.Pet); // PET
                Packet.Builder.HellstormStatusCommand(GameSession); // Rocket launcher

                //MBA -> MenuButtonAccess
                //DB -> Disable button
                //EB -> Enable button
                //Packet.Builder.LegacyModule(GameSession, "0|UI|MBA|DB|7");
                //Packet.Builder.LegacyModule(GameSession, "0|UI|MBA|DB|6");
                //Packet.Builder.LegacyModule(GameSession, "0|UI|MBA|DB|2");
                Packet.Builder.LegacyModule(GameSession, "0|UI|MV|HM|4");
                //Packet.Builder.LegacyModule(GameSession, "0|UI|MBA|DB|5");

                if (GameSession.Player.Group != null)
                    Packet.Builder.GroupInitializationCommand(GameSession); // group

                if (GameSession.Player.Information.Title != null)
                    Packet.Builder.TitleCommand(GameSession, GameSession.Player); // title
                
                GameSession.Player.Information.Premium.Login(GameSession); // Premium notification
                
                Packet.Builder.QuestInitializationCommand(GameSession); // Quests

                CreateFormations(GameSession); // Drone Formations
                
                CreateTechs(GameSession); // Techs
                
                CreateAbilities(GameSession); // Abilities
                
                Packet.Builder.AttributeOreCountUpdateCommand(GameSession, GameSession.Player.Information.Cargo); // Cargo

                UpdateClanWindow(GameSession); // Clan Window
                
                Packet.Builder.EventActivationStateCommand(GameSession, 0, true); // Event Christmas 0
                Packet.Builder.EventActivationStateCommand(GameSession, 1, true); // Event Christmas 1
            }
            catch (Exception e)
            {
                Console.WriteLine("legacy:");
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
            }
        }

        public static void UpdateClanWindow(GameSession gameSession)
        {
            foreach (var member in gameSession.Player.Clan.Members)
            {
                var memberPlayer = member.Value.Player;
                if (memberPlayer != null)
                {
                    var memberSession = memberPlayer.GetGameSession();
                    Packet.Builder.ClanWindowInitCommand(memberSession);
                }
            }
        }

        public void InitiateEvents()
        {
            foreach (var gameEvent in World.StorageManager.Events)
            {
                if (gameEvent.Value.Active)
                    World.DatabaseManager.LoadEventForPlayer(gameEvent.Key, GameSession.Player);
            }
            foreach (var gameEvent in GameSession.Player.EventsPraticipating)
                gameEvent.Value.Start();
        }

        private static void CreateTechs(GameSession session)
        {
            session.Player.Techs.Add(new RocketPrecission(session.Player));
            session.Player.Techs.Add(new ShieldBuff(session.Player));
            session.Player.Techs.Add(new BattleRepairRobot(session.Player));
            session.Player.Techs.Add(new EnergyLeech(session.Player));
            session.Player.Techs.Add(new ChainImpulse(session.Player));

            Packet.Builder.TechStatusCommand(session);
        }

        private static void CreateFormations(GameSession session)
        {
            Packet.Builder.DroneFormationAvailableFormationsCommand(session);
        }

        private static void CreateAbilities(GameSession session)
        {
            Packet.Builder.AbilityStatusFullCommand(session, session.Player.Abilities);
        }
    } 
}
