namespace HunterGameServer
{
    internal class Program
    {
        public static ServerSocket server;

        static void Main(string[] args)
        {
            //TerrainMsg msg = new TerrainMsg();
            //TerrainMsg msg1 = new TerrainMsg();
            //BattleRoom room = new BattleRoom();
            //msg = room.Test();

            //byte[] bytes= msg.Writing();

            //msg1.Reading(bytes,8);



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
                //if (input.Substring(0, 2) == "B:")
                //{
                //    EnemyNameMsg stringMsg = new EnemyNameMsg();
                //    stringMsg.name = input.Substring(2);
                //    serverSocket.Send(stringMsg);
                //}
            }
        }
    }
}
