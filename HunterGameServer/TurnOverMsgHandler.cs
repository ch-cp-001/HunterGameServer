using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HunterGameServer
{
    internal class TurnOverMsgHandler : BaseHandler
    {
        public override void Handle(ClientSocket socket)
        {
            Console.WriteLine("处理turnover消息");
            TurnOverMsg msg = message as TurnOverMsg;
            if (msg != null)
            {
                
                BattleRoom battleRoom = BattleRoomMgr.Instance.GetBattleRoom(msg.roomId);
                if(battleRoom != null) 
                    battleRoom.BattleResCheck();
            }
        }
    }
}
