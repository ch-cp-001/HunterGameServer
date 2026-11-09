using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HunterGameServer
{
    internal class OperationMsgHandler : BaseHandler
    {
        public override void Handle(ClientSocket socket)
        {
            Console.WriteLine("处理移动请求消息");
            // 根据位置计算可移动范围
            OperationMsg msg = message as OperationMsg;
            BattleRoom battleRoom = BattleRoomMgr.Instance.GetBattleRoom(msg.roomId);
            if (battleRoom != null)
            {
                if(msg.operation == 0)
                {
                    MoveScopeMsg scopeMsg = new MoveScopeMsg();
                    scopeMsg.moves = battleRoom.GetMoveScope(msg.type);
                    socket.MySendAsync(scopeMsg);
                }
                else if(msg.operation == 1)
                {
                    battleRoom.PlayerWait(msg.type);
                }
            }
        }
    }
}
