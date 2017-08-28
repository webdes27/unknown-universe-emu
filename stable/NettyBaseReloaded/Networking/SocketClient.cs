﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NettyBaseReloaded.Networking
{
    class SocketClient
    {
        private XSocket XSocket;

        public SocketClient(XSocket gameSocket)
        {
            XSocket = gameSocket;
            XSocket.OnReceive += XSocketOnOnReceive;
            XSocket.Read(true);
        }

        private void XSocketOnOnReceive(object sender, EventArgs e)
        {
            var packetArgs = (StringArgs) e;
            Socketty.PacketHandler.Handle(packetArgs.Packet);
        }
    }
}
