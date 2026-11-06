using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HunterGameServer
{
    internal class QuitMsgHandler : BaseHandler
    {
        public override void Handle(ClientSocket socket)
        {
            Console.WriteLine("收到客户端{0}断开连接消息", socket.id);
            // 在断开连接之前先退出账号登陆状态
            if(!string.IsNullOrEmpty(socket.GetUserName()))
                UserMgr.Instance.ChangeUserState(socket.GetUserName(),UserState.Offline);
            // 告知服务端该socket需要断开
            Program.server.AddDelScoList(socket.id);
        }
    }
}
