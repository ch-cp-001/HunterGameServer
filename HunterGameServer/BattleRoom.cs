using System.Text;
using static System.Formats.Asn1.AsnWriter;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HunterGameServer
{
    // 玩家操作类型
    enum PlayerDo
    {
        Move,
        Atk,
        Wait,
    }

    // 专门用于创建战斗房间的类，检测usermgr中匹配中用户，选择何时用户创建战斗房间并将他们加入房间中
    internal class BattleRoom
    {
        private int roomId;
        // 直接设定1v1
        // 猎人
        private PlayerData hunter;
        int hunterId;
        // 猎物
        private PlayerData prey;
        int preyId;
        // 地图长宽和数据
        private TerrainData terrain;

        // 发送消息给客户端告知匹配成功
        private MatchMsg matchMsg;

        // 记录当前可移动范围
        private List<int> hunterMoveScope;
        private List<int> preyMoveScope;

        // 计算移动范围时使用的坐标
        private int scopeX = -1;
        private int scopeY = -1;
        private bool preType; // 记录上次计算可移动范围的对象类型，更多人时可采用id或账户区别玩家

        private int hunterTurn = 0;   // 回合数
        private PlayerDo hunterDo;  // 当前回合猎人操作
        MovePathMsg hunterMoveMsg = new MovePathMsg();
        private int hunterMoveX;    // 记录猎人本次移动目标地点
        private int hunterMoveY;


        private int preyTurn = 0;   // 回合数
        private PlayerDo preyDo;
        MovePathMsg preyMoveMsg = new MovePathMsg();
        private int preyMoveX;
        private int preyMoveY;

        private int maxTurn = 10;   // 游戏结束回合数

        public BattleRoom(int roomId)
        {
            this.roomId = roomId;
        }

        // 向房间中加入用户，随机分配角色
        public void Adduser(string username)
        {
            bool tmp = false;// 标识加入的角色
            // 发送消息给客户端告知匹配成功
            matchMsg = new MatchMsg();
            matchMsg.flag = true;
            matchMsg.username = username;
            if (string.IsNullOrEmpty(username)) return;
            // 房间中加入的第一个角色随机猎人或者猎物，第二个角色选择与第一个角色不同的
            if(hunter == null && prey == null)
            {
                Random ran = new Random();
                int i = ran.Next(100);
                if (i < 50)
                {
                    tmp = true;
                }
                else
                {
                    tmp = false;
                }
            }else if (hunter == null)
            {
                tmp = true;
            }
            else if(prey == null)
            {
                tmp = false;
            }

            if (tmp)
            {
                Console.WriteLine("加入角色"+username+"，身份为猎人");
                hunter = new PlayerData();
                hunter.username = username;
                hunterId = UserMgr.Instance.GetUserSocket(hunter.username);
                Program.server.Send(hunterId, matchMsg);
            }
            else
            {
                Console.WriteLine("加入角色"+username+"，身份为猎物");
                prey = new PlayerData();
                prey.username = username;
                preyId = UserMgr.Instance.GetUserSocket(prey.username);
                Program.server.Send(preyId, matchMsg);
            }
        }

        // 战斗准备
        public void Prepare()
        {
            CreateTerrainData();
            Task.Delay(1000);
            SendStartMsg();
        }

        // 生成地图数据
        public void CreateTerrainData()
        {
            terrain = new TerrainData();
            terrain.width = 5;
            terrain.height = 5;
            terrain.map = new List<TerrainType>();
            for (int i = 0; i < terrain.height; i++)
            {
                for (int j = 0; j < terrain.width; j++)
                {
                    Random random = new Random();
                    int ran = random.Next(100);
                    if (ran < 10)
                    {
                        terrain.map.Add(TerrainType.Obstacle);
                    }
                    terrain.map.Add(TerrainType.Normal);
                }
            }
            // 初始化两个角色位置，猎物在左侧中间，猎人在右侧中间
            prey.posX = 0;
            prey.posY = terrain.height / 2;
            hunter.posX = terrain.width-1;
            hunter.posY = terrain.height / 2;
            // 初始位置不能是障碍
            terrain.map[prey.posY * terrain.width + prey.posX] = TerrainType.Normal;
            terrain.map[hunter.posY * terrain.width + hunter.posX] = TerrainType.Normal;
        }

        // 向客户端发送地图数据
        public void SendStartMsg()
        {
            Console.WriteLine("发送战斗房间消息");
            PlayerMsg hunterMsg = new PlayerMsg();
            hunterMsg.data = hunter;
            PlayerMsg preyMsg = new PlayerMsg();
            preyMsg.data = prey;
            // 向hunter发送消息
            Program.server.Send(hunterId, hunterMsg);
            Program.server.Send(hunterId, preyMsg);
            // 敌人的名字发一下用于显示在战斗场景中
            EnemyNameMsg nameMsg = new EnemyNameMsg();
            nameMsg.name = UserMgr.Instance.GetUserData(prey.username).nickName;
            Program.server.Send(hunterId, nameMsg);
            // 发送roomid
            RoomIdMsg roomIdMsg = new RoomIdMsg();
            roomIdMsg.roomId = roomId;
            Program.server.Send(hunterId,roomIdMsg);
            // 向prey发送数据
            Program.server.Send(preyId, preyMsg);
            Program.server.Send(preyId, hunterMsg);
            nameMsg.name = UserMgr.Instance.GetUserData(hunter.username).nickName;
            Program.server.Send(preyId, nameMsg);
            Program.server.Send(preyId, roomIdMsg); 
            // 发送地图数据
            TerrainMsg terrainMsg = new TerrainMsg();
            terrainMsg.data = terrain;
            Program.server.Send(hunterId, terrainMsg);
            Program.server.Send(preyId, terrainMsg);
        }

        //计算用户可移动范围
        public List<int> GetMoveScope(bool type)
        {
            int movelength = type ? 2 : 1 + 1;
            // 猎人
            if (type)
            {
                if (scopeX == hunter.posX && scopeY == hunter.posY && preType)
                    return hunterMoveScope;
                scopeX = hunter.posX;
                scopeY = hunter.posY;
                hunterMoveScope.Clear();
                BFS(hunter.posX, hunter.posY, movelength, type);
                return hunterMoveScope;
            }
            // 猎物
            else
            {
                if (scopeX == prey.posX && scopeY == prey.posY && !preType)
                    return preyMoveScope;
                scopeX = prey.posX;
                scopeY = prey.posY;
                preyMoveScope.Clear();
                BFS(prey.posX, prey.posY, movelength, type);
                return preyMoveScope;
            }           
            
        }

        /// <summary>
        /// 广度优先搜索，确定可行走范围。
        /// </summary>
        /// <param name="x">坐标x</param>
        /// <param name="y">坐标y</param>
        /// <param name="num">剩余行动次数</param>
        private void BFS(int x, int y, int num,bool type)
        {
            // 坐标不合法直接退出
            if (!CheckPos(x, y)) return;
            // 递归退出条件
            if (num == 0) return;
            int index = y * terrain.width + x;
            // 自己、障碍、已经记录过的不再处理
            if (terrain.map[index] == TerrainType.Obstacle) return;
            
            if (type)
            {
                if (hunterMoveScope.Contains(index)) return;
                if (hunter.posX == x && hunter.posY == y) return;
                hunterMoveScope.Add(index);
            }
            else
            {
                if (preyMoveScope.Contains(index)) return;
                if ((hunter.posX == x && hunter.posY == y) || (prey.posX == x && prey.posY == y))
                    return;
                preyMoveScope.Add(index);
            }                       

            num--;
            if (num > 0)
            {
                // 对周围四个格子进行广度搜索
                BFS(x + 1, y, num, type);
                BFS(x, y + 1, num, type);
                BFS(x - 1, y, num, type);
                BFS(x, y - 1, num, type);
            }
        }

        // 检查坐标是否合法
        private bool CheckPos(int x,int y)
        {
            if (x >= 0 && x < terrain.width && y >= 0 && y < terrain.height)
                return true;
            return false;
        }

        // 检查目标位置是否可以移动
        public bool CheckMovePos(int x,int y,bool type)
        {
            int index = x + y * terrain.width;
            // 先判断坐标是否合法
            if (CheckPos(x, y))
            {
                if (type)
                {
                    return hunterMoveScope.Contains(index);
                }
                else
                {
                    return preyMoveScope.Contains(index);
                }
            }
            return false;
        }

        public void GetPath(int endX,int endY,bool type)
        {
            // 先判断是否可以移动，然后A*寻路
            if (CheckMovePos(endX, endY, type))
            {
                ASharpNavigation.Instance.SetTerrain(GetTerrainData());
                if (type)
                {
                    hunterMoveMsg.path = ASharpNavigation.Instance.GetPath(hunter.posX,hunter.posY,endX,endY);
                    
                    hunterTurn++;
                    hunterDo = PlayerDo.Move;
                    hunterMoveX = endX;
                    hunterMoveY = endY;
                }
                else
                {
                    preyMoveMsg.path = ASharpNavigation.Instance.GetPath(prey.posX, prey.posY, endX, endY);
                    //Program.server.Send(preyId, msg);
                    preyTurn ++;
                    preyDo = PlayerDo.Move;
                    preyMoveX = endX;
                    preyMoveY = endY;
                }
                // 路径数据发送回客户端

            }
            else
            {
                // 不可移动，立即通知客户端，不计入回合数
                BoolMsg msg = new BoolMsg();
                msg.flag = false;
                Program.server.Send(type? hunterId:preyId, msg);
            }

            // 增加对应回合数，当两边回合数相同时再发送操作信息
            if (hunterTurn == preyTurn)
                SendDoMsg();
        }
        
        // 向客户端发送玩家操作消息
        private void SendDoMsg()
        {
            switch (hunterDo)
            {
                case PlayerDo.Move:
                    Program.server.Send(hunterId, hunterMoveMsg);
                    EnemyPosMsg enemyPosMsg = new EnemyPosMsg();
                    enemyPosMsg.x = hunterMoveX;
                    enemyPosMsg.y = hunterMoveY;
                    Program.server.Send(preyId, enemyPosMsg);    // 将猎人本次移动目标点发给猎物，用于再猎物侧显示敌人移动
                    break;
                case PlayerDo.Atk:
                    break;
                case PlayerDo.Wait:
                    break;
                default:
                    break;
            }

            switch (preyDo)
            {
                case PlayerDo.Move:
                    Program.server.Send(preyId, preyMoveMsg);
                    EnemyPosMsg enemyPosMsg = new EnemyPosMsg();
                    enemyPosMsg.x = preyMoveX;
                    enemyPosMsg.y = preyMoveY;
                    Program.server.Send(hunterId, enemyPosMsg);    // 将猎物本次移动目标点发给猎人，用于再猎人侧显示敌人移动
                    break;
                case PlayerDo.Atk:
                    break;
                case PlayerDo.Wait:
                    break;
                default:
                    break;
            }

            // 发送回合数信息
            BattleTurnMsg msg = new BattleTurnMsg();
            msg.turn = hunterTurn;
            msg.maxTurn = maxTurn;
            Program.server.Send(hunterId,msg);
            Program.server.Send(preyId,msg);
        }


        public TerrainData GetTerrainData()
        {
            return terrain;
        }

        //public TerrainMsg Test()
        //{
        //    CreateTerrainData();

        //    TerrainMsg msg = new TerrainMsg();
        //    msg.data = terrain;
        //    return msg;
        //}

    }


}
