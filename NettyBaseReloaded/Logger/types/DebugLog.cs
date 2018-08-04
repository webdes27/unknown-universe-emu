﻿using NettyBaseReloaded.Logger;
using System;
using System.Collections.Generic;
using System.IO;

namespace NettyBaseReloaded
{
    class DebugLog : Log
    {
        public const string SUB_DIR = "/debug/";

        public DebugLog(string fileName)
        {
            Initialize(fileName);
        }

        public override void Initialize(string fileName)
        {
            Writer = new Writer(Directory.GetCurrentDirectory() + BASE_DIR + "/$SERVER_SESSION$" + SUB_DIR + fileName);
        }

        public void Write(string message)
        {
            try
            {
                Writer.Write(DateTime.Now + " - " + "(" + Out.GetCaller() + ") " + message);
                LastLogTime = DateTime.Now;
            }
            catch (Exception)
            {
                
            }
        }
    }
}
