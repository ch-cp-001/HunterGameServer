namespace HunterGameServer
{
    internal class MatchMsgHandler : BaseHandler
    {
        public override void Handle(ClientSocket socket)
        {
            Console.WriteLine("处理匹配信息");
            MatchMsg msg = message as MatchMsg;
            if(UserMgr.Instance.CheckExist(msg.username))
            {
                // true表示开始匹配
                if (msg.flag && UserMgr.Instance.GetUserState(msg.username) != UserState.Maching)
                {
                    // 将玩家状态修改为匹配中
                    UserMgr.Instance.ChangeUserState(msg.username, UserState.Maching);
                    // 看下当前匹配线程是否开启，未开启则开启
                    if(!BattleRoomMgr.Instance.IsStart())
                    {
                        BattleRoomMgr.Instance.StartMatch();
                    }
                }
                // 表示取消匹配
                else if(UserMgr.Instance.GetUserState(msg.username) == UserState.Maching)
                {
                    // 将玩家状态修改为在线
                    UserMgr.Instance.ChangeUserState(msg.username, UserState.Online);
                }                    
            }
            else
            {
                Console.WriteLine("接收到的用户信息不存在");
            }
        }
    }
}
