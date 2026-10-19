namespace HunterGameServer
{
    internal class UserMsgHandler : BaseHandler
    {
        public override void Handle(ClientSocket socket)
        {
            UserMsg msg = message as UserMsg;
            Console.WriteLine(msg.data.flag + "账号：" + msg.data.userName + "，密码:" + msg.data.password+"，昵称："+msg.data.nickName+"，等级："+msg.data.lev);
            // 客户端注册 比较本地是否已存在当前账户
            BoolMsg boolMsg = new BoolMsg();
            if (msg.data.flag == 0)
            {
                // 当前账户已存在,注册失败
                if (UserMgr.Instance.CheckExist(msg.data.userName))
                {
                    boolMsg.flag = false;
                    Console.WriteLine("注册失败");
                }
                else
                {
                    UserMgr.Instance.Add(msg.data);
                    boolMsg.flag = true;
                    Console.WriteLine("注册成功");
                }
                socket.MySendAsync(boolMsg);
            }
            // 客户端登录逻辑
            else if(msg.data.flag == 1)
            {
                if(UserMgr.Instance.Check(msg.data.userName, msg.data.password))
                {
                    boolMsg.flag = true;
                    // 将账户信息发送给客户端
                    msg.data = UserMgr.Instance.GetUserData(msg.data.userName);

                }
                else
                    boolMsg.flag = false;
                Console.WriteLine("登录"+ boolMsg.flag  );
                socket.MySendAsync(boolMsg);
                socket.MySendAsync(msg);                
            }
            // 修改昵称
            else if (msg.data.flag == 2)
            {
                // 这里暂时就不做检测了
                UserMgr.Instance.ModifyNickName(msg.data.userName, msg.data.nickName);
                boolMsg.flag = true;
                socket.MySendAsync(boolMsg);
            }
            // 修改密码
            else if(msg.data.flag == 3)
            {
                UserMgr.Instance.ModifyPwd(msg.data.userName, msg.data.password);
                boolMsg.flag = true;
                socket.MySendAsync(boolMsg);
            }
        }
    }
}
