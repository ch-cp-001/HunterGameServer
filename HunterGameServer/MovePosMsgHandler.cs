using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HunterGameServer
{
    internal class MovePosMsgHandler : BaseHandler
    {
        public override void Handle(ClientSocket socket)
        {
            Console.WriteLine("处理移动目标消息");
            MovePosMsg msg = message as MovePosMsg;
            if (msg != null)
            {
                BattleRoom room = BattleRoomMgr.Instance.GetBattleRoom(msg.roomId);
                room.GetPath(msg.x, msg.y, msg.type);
            }
        }
    }
}
