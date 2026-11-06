
using System.Net;
using System.Net.Sockets;

namespace HunterGameServer
{
    // serversocket负责异步接收客户端连接，当接收到连接后会创建一个clientsocket用于存放链接。在clientsocket中接收和处理消息
    internal class ServerSocket
    {
        public Socket socket;
        public Dictionary<int,ClientSocket> clientSocketDic = new Dictionary<int, ClientSocket>();
        public bool isClose;

        // 待删除socket列表
        public List<int> delSocLists = new List<int>();

        SocketAsyncEventArgs acceptE=null;

        public void Start(string ip,int point,int num)
        {
            socket = new Socket(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.Tcp);
            socket.Bind(new IPEndPoint(IPAddress.Parse(ip),point));
            socket.Listen(num);
            isClose = false;

            acceptE = new SocketAsyncEventArgs();
            MyAcceptAsync();
            Task.Run(DeleteSocket);
            Console.WriteLine("服务器启动成功");
        }

        // 清除待删除列表中的所有socket
        private void DeleteSocket()
        {
            while (!isClose)
            {
                if (delSocLists.Count > 0)
                {
                    lock (clientSocketDic)
                    {
                        for (int i = 0; i < delSocLists.Count; i++)
                        {
                            if (clientSocketDic.ContainsKey(delSocLists[i]))
                            {
                                clientSocketDic[delSocLists[i]].Close();
                                clientSocketDic.Remove(delSocLists[i]);
                                Console.WriteLine("连接删除：" + delSocLists[i]);
                            }
                        }
                        delSocLists.Clear();
                    }
                }
                // 线程休眠，降低执行次数
                Thread.Sleep(1000);
            }
        }

        // 异步获取连接
        public void MyAcceptAsync()
        {
            if (!isClose){
                acceptE.Completed += (s, args) =>
                {
                    if (args.SocketError == SocketError.Success)
                    {
                        ClientSocket clientSocket = new ClientSocket(args.AcceptSocket);
                        //EnemyNameMsg msg = new EnemyNameMsg();
                        //msg.name = "欢迎登录猎人游戏服务器";
                        //clientSocket.MySendAsync(msg);
                        Console.WriteLine("本次连接的socketId为"+clientSocket.id);
                        clientSocketDic.Add(clientSocket.id, clientSocket);

                        if (!isClose)
                        {
                            acceptE.AcceptSocket = null;
                            socket.AcceptAsync(acceptE);
                        }
                    }
                    else
                    {
                        Console.WriteLine("获取连接失败{0}", args.SocketError);
                    }
                };
                socket.AcceptAsync(acceptE);
            }            
        }

        // 服务端向所有客户端发送消息
        public void Send(BaseMsg msg)
        {
            lock (clientSocketDic)
            {
                foreach (ClientSocket item in clientSocketDic.Values)
                {
                    item.MySendAsync(msg);
                }
            }
        }

        public void Send(int socketId, BaseMsg msg)
        {
            if (clientSocketDic.ContainsKey(socketId))
            {
                clientSocketDic[socketId].MySendAsync(msg);
            }
        }

        // 关闭服务器
        public void Close()
        {
            // 
            Console.WriteLine("关闭服务器");
            isClose = true;
            // 断开所有客户端链接
            lock (clientSocketDic)
            {
                foreach (ClientSocket item in clientSocketDic.Values)
                {
                    item.Close();
                }
            }
            
            clientSocketDic.Clear();
            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
        }

        // 添加待断开连接
        public void AddDelScoList(int id)
        {
            delSocLists.Add(id);
        }
    }
}
