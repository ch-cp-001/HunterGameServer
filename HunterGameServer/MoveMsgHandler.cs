using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HunterGameServer
{
    internal class MoveMsgHandler : BaseHandler
    {
        public override void Handle(ClientSocket socket)
        {
            Console.WriteLine("处理移动请求消息");
            // 根据位置计算可移动范围
            MoveMsg msg = message as MoveMsg;
            BattleRoom battleRoom = BattleRoomMgr.Instance.GetBattleRoom(msg.roomId);
            if (battleRoom != null)
            {
                MoveScopeMsg scopeMsg = new MoveScopeMsg();
                scopeMsg.moves = battleRoom.GetMoveScope(msg.type);
                socket.MySendAsync(scopeMsg);
            }
        }
    }
}
