﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NettyBaseReloaded.Main.commands
{
    class Debug : Command
    {
        public Debug() : base("debug", "Debug command")
        {
            
        }

        public override void Execute(string[] args = null)
        {
            if (args == null)
            {
                Console.WriteLine("Debug::No parameters");
                return;
            }
            switch (args[1])
            {
                case "commands":
                case "printcmd":
                case "printcmds":
                    if (Properties.Game.PRINTING_COMMANDS)
                    {
                        Properties.Game.PRINTING_COMMANDS = false;
                        Console.WriteLine("Debug::Stopped printing commands");
                        break;
                    }
                    Properties.Game.PRINTING_COMMANDS = true;
                    Console.WriteLine("Debug::Commands should now print");
                    break;
                case "packets":
                case "printpacket":
                case "printpackets":
                    if (Properties.Game.PRINTING_LEGACY_COMMANDS)
                    {
                        Properties.Game.PRINTING_LEGACY_COMMANDS = false;
                        Console.WriteLine("Debug::Stopped printing legacy commands");
                        break;
                    }

                    Properties.Game.PRINTING_LEGACY_COMMANDS = true;
                    Console.WriteLine("Debug::Legacy commands should now print");
                    break;
                case "range":
                case "entities":
                    if (Properties.Game.DEBUG_ENTITIES)
                    {
                        Properties.Game.DEBUG_ENTITIES = false;
                        Console.WriteLine("Debug::Stopped printing range entities");
                        break;
                    }

                    Properties.Game.DEBUG_ENTITIES = true;
                    Console.WriteLine("Debug::Range entities should now print");
                    break;
            }
        }
    }
}
