﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chamou.TcpProxy
{
    class Program
    {
        static void Main(string[] args)
        {
            var server = new TcpProxyServer();
            server.Start();
            server.Join();
        }
    }
}
