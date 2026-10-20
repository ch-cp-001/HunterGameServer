namespace HunterGameServer
{
    internal class Program
    {
        public static ServerSocket server;

        static void Main(string[] args)
        {
            //UserData data = new UserData();
            //data.flag = 0;
            //data.nickName = "未命名";
            //data.userName = "Cc";
            //data.password = "11";
            //data.lev = 1;
            //UserData data1 = new UserData();
            //data1.flag = 2;
            //data1.nickName = "在这种";
            //data1.userName = "Cd";
            //data1.password = "11";
            //data1.lev = 5;

            //UserMgr.Instance.Add(data);
            //UserMgr.Instance.Add(data1);
            //UserMgr.Instance.Serialize();

            //UserMgr.Instance.DeSerialize();
            //Dictionary<string, UserData> userDic = UserMgr.Instance.userInfos.userDic;
            //foreach (string s in userDic.Keys)
            //{
            //    Console.WriteLine("账户" + s + ":" + userDic[s].password + "-" + userDic[s].nickName + "-" + userDic[s].lev);
            //}

            //Console.ReadLine();
            ServerSocket serverSocket = new ServerSocket();
            server = serverSocket;
            serverSocket.Start("127.0.0.1", 8080, 10);
            int i = 1;
            string input = "";
            while (true)
            {
                input = Console.ReadLine();
                if (input == "Quit")
                {
                    serverSocket.Close();
                }
                if (input.Substring(0, 2) == "B:")
                {
                    StringMsg stringMsg = new StringMsg();
                    stringMsg.str = input.Substring(2);
                    serverSocket.Send(stringMsg);
                }
            }
        }
    }
}
