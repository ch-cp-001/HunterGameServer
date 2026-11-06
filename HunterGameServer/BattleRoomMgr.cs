using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HunterGameServer
{
    // 专门管理battleRoom的类，
    internal class BattleRoomMgr
    {
        private static BattleRoomMgr instance = new BattleRoomMgr();
        public static BattleRoomMgr Instance => instance;

        private int roomId=0;
        // roomId,room
        private Dictionary<int,BattleRoom> rooms = new Dictionary<int,BattleRoom>();

        private List<string> matchUsers;

        private bool isStart;

        public bool IsStart()
        {
            return isStart;
        }

        public void StartMatch()
        {
            isStart = true;
            Task.Run(MatchUser);
        }

        //匹配用户方法，开一个线程不断检测匹配中用户超过2人时并且等级相近（差值<=10，暂不考虑）就创建战斗房间将这两个加入其中

        // 一个线程专门用于创建对战房间，筛选合适的匹配中用户，创建一个战斗房间类，用户随机分配猎人猎物角色，
        // 生成地图，初始化玩家位置，
        // 将地图数据，玩家位置发送给客户端
        private void MatchUser()
        {
            while (true)
            {
                matchUsers = UserMgr.Instance.GetMatchUsers();
                if (matchUsers != null && matchUsers.Count >= 2)
                {
                    string name1 = matchUsers[0];
                    string name2 = matchUsers[1];
                    Console.WriteLine("创建战斗房间"+roomId+"，加入"+ name1 +"和"+ name2);
                    BattleRoom room = new BattleRoom(roomId);
                    room.Adduser(name1);
                    UserMgr.Instance.ChangeUserState(name1,UserState.Battling);
                    room.Adduser(name2);
                    UserMgr.Instance.ChangeUserState(name2,UserState.Battling);
                    room.Prepare();                    
                    rooms.Add(roomId++, room);
                }

                Task.Delay(1000);
            }
        }

        public BattleRoom GetBattleRoom(int roomId)
        {
            if(rooms.ContainsKey(roomId))
                return rooms[roomId];
            return null;
        }
    }
}
