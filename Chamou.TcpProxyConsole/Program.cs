using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chamou.TcpProxy;

namespace Chamou.TcpProxyConsole
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
