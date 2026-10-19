namespace HunterGameServer
{
    internal class Program
    {
        public static ServerSocket server;

        static void Main(string[] args)
        {
            ServerSocket serverSocket = new ServerSocket();
            server = serverSocket;
            serverSocket.Start("127.0.0.1",8080,10);


            int i = 1;
            string input = "";
            while (true)
            {
                input = Console.ReadLine();
                if(input == "Quit")
                {
                    serverSocket.Close();
                }
                if(input.Substring(0,2) == "B:")
                {
                    StringMsg stringMsg = new StringMsg();
                    stringMsg.str = input.Substring(2);
                    serverSocket.Send(stringMsg);
                }
            }
        }
    }
}
