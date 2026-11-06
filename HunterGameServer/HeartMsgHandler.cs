using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HunterGameServer
{
    internal class HeartMsgHandler : BaseHandler
    {
        public override void Handle(ClientSocket socket)
        {
            socket.frontHeartTime = DateTime.Now.Ticks / TimeSpan.TicksPerSecond;
            Console.WriteLine("收到客户端{0}心跳消息，此时fronttime={1}", socket.id, socket.frontHeartTime);
        }
    }
}
