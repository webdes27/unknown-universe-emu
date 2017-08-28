﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NettyBaseReloaded.Networking
{
    class Server
    {
        public const int GAME_PORT = 8080;
        public const int BROWSER_CLIENT = 9772;
        public const int CHAT_PORT = 9338;
        public const int SOCKET_PORT = 1337;
        public const int POLICY_PORT = 843;

        private XSocket serverSocket;
        private int Port;

        public Server(int port)
        {
            serverSocket = new XSocket(port);
            serverSocket.OnAccept += ServerSocketOnOnAccept;
            serverSocket.Listen();
            Port = port;
        }

        private void ServerSocketOnOnAccept(object sender, XSocketArgs xSocketArgs)
        {
            switch (Port)
            {
                case GAME_PORT:
                    new GameClient(xSocketArgs.XSocket);
                    break;
                case CHAT_PORT:
                    new ChatClient(xSocketArgs.XSocket);
                    break;
                case BROWSER_CLIENT:
                    throw new NotImplementedException();
                case SOCKET_PORT:
                    new SocketClient(xSocketArgs.XSocket);
                    break;
                case POLICY_PORT:
                    new PolicyClient(xSocketArgs.XSocket);
                    break;
            }
        }
    }
}
