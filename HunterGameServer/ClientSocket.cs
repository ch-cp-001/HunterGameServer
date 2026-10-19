using System.Net.Sockets;
using System.Runtime.InteropServices;

namespace HunterGameServer
{
    // 服务端连接客户端的socket
    public class ClientSocket
    {
        public static int SOCKET_INDEX = 1;
        public int id;
        public Socket socket;
        public bool isConnect => socket.Connected;

        // 缓存字节数组，用于解决分包粘包问题
        public byte[] cacheBytes = new byte[1024 * 1024];
        public int cacheBytesNum;


        // 心跳消息超时时间
        public static int TIMEOUT_HEARTMSG = 60;
        // 心跳消息上次时间
        public long frontHeartTime = -1;

        public SocketAsyncEventArgs receiveE;
        public SocketAsyncEventArgs sendE;
        
        // 发送的消息id
        private int msgId= 0;

        public ClientSocket(Socket socket)
        {
            this.socket = socket;
            id = SOCKET_INDEX;
            SOCKET_INDEX++;

            receiveE = new SocketAsyncEventArgs();
            receiveE.Completed += (s, args) =>
            {
                Console.WriteLine("接收到消息");
                HandleReceiveMsg(args.Buffer, args.BytesTransferred);

                if (isConnect)
                {
                    args.SetBuffer(0, args.Buffer.Length);
                    socket.ReceiveAsync(receiveE);
                }
            };
            sendE = new SocketAsyncEventArgs();
            sendE.Completed += (s, args) =>
            {
                if (args.SocketError == SocketError.Success)
                {
                    Console.WriteLine("发送成功");
                }
                else
                {
                    Console.WriteLine("发送失败{0}", args.SocketError);
                }
            };
            MyReceiveAsync();
        }
        // 断开连接
        public void Close()
        {
            if(socket != null && isConnect)
            {
                Console.WriteLine("关闭客户端{0}连接",id);
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
                socket = null;
            }
        }

        // 处理接收到的消息，根据缓存的消息长度先解析消息体类型和消息体长度，然后解析消息体
        public void HandleReceiveMsg(byte[] receiveBytes, int receiveNum)
        {
            int typeId = 0;     // 类型ID
            int bytesNum = 0;   // 消息字节长度
            int index = 0;      // 已处理消息长度
            // 将接收到的消息复制到消息缓存中
            Array.Copy(receiveBytes, 0, cacheBytes, cacheBytesNum, receiveNum);
            cacheBytesNum += receiveNum;

            while (true)
            {
                bytesNum = -1;
                // 缓存中的字节长度超过8可以解析类型和长度
                if (cacheBytesNum - index >= 8)
                {
                    // 类型ID
                    typeId = BitConverter.ToInt32(cacheBytes, index);
                    index += 4;
                    if (typeId == 1001)
                        Console.WriteLine("处理用户信息");
                    // 消息字节长度
                    bytesNum = BitConverter.ToInt32(cacheBytes, index);
                    index += 4;
                }
                // 已经解析类型和长度 并且剩余字节数量超过记录的数据长度  则可以解析数据
                if (bytesNum >= 0 && cacheBytesNum - index >= bytesNum)
                {
                    BaseMsg baseMsg = null;
                    if (typeId == 1010)
                        baseMsg = new QuitMsg();
                    else if(typeId == 999)
                        baseMsg = new HeartMsg();
                    else
                    {
                        baseMsg = MsgPool.Instance.GetMsg(typeId);
                        baseMsg.Reading(cacheBytes, index);
                    }
                    index += bytesNum;
                    if (baseMsg != null)
                        ThreadPool.QueueUserWorkItem(Handle, baseMsg);
                    // 正好处理完了，直接将长度设置为0，相当于清空了缓存字节数组
                    if (cacheBytesNum == index)
                    {
                        cacheBytesNum = 0;
                        break;
                    }
                }
                else
                {
                    // 说明已经解析了类型和长度但是后面消息体部分长度不够，那么需要将index复位等待下次消息传入后再处理
                    if (bytesNum != -1)
                    {
                        index -= 8;
                    }
                    // 多余的部分放进缓存字节数组开头，后面再用
                    Array.Copy(cacheBytes, index, cacheBytes, 0, cacheBytesNum - index);
                    cacheBytesNum = cacheBytesNum - index;
                    break;
                }
            }

        }
        // 具体处理消息方法
        public void Handle(object obj)
        {
            BaseMsg msg = obj as BaseMsg;
            if(msg.GetTypeId() == 1010)
            {
                Console.WriteLine("客户端{0}断开连接", id);
                Program.server.AddDelScoList(this);
                Close();
            }
            else if (msg.GetTypeId() == 999)
            {
                frontHeartTime = DateTime.Now.Ticks / TimeSpan.TicksPerSecond;
                Console.WriteLine("收到心跳消息");
            }
            else
            {
                BaseHandler handler = MsgPool.Instance.GetHandler(msg.GetTypeId());
                if (handler != null)
                {
                    handler.message = msg;
                    handler.Handle(this);
                }
                else
                {
                    Console.WriteLine(msg.GetTypeId() + "类型数据没有相应处理方法，请添加");
                }
            }
        }

        // 检查链接的心跳状态，超时则断开连接
        public void CheckHeartMsg()
        {
            long time = DateTime.Now.Ticks / TimeSpan.TicksPerSecond - frontHeartTime;
            if (frontHeartTime != -1 && time > TIMEOUT_HEARTMSG)
            {
                Program.server.AddDelScoList(this);
            }
        }

        // 异步获取消息
        public void MyReceiveAsync()
        {
            if (isConnect)
            {
                byte[] bytes = new byte[1024 * 1024];
                receiveE.SetBuffer(bytes,0,bytes.Length);
                
                socket.ReceiveAsync(receiveE);
            }
        }

        // 异步发送消息
        public void MySendAsync(BaseMsg msg)
        {
            if (isConnect)
            {
                msg.id = msgId++;
                sendE.SetBuffer(msg.Writing());
                Console.WriteLine("向客户端"+id+"发送消息"+msg.id);
                socket.SendAsync(sendE);
            }
        }
    }
}
