using MessagePack;

namespace HunterGameServer
{
    // 二进制序列化工具，没连数据库先序列化成文件放本地
    class SerializerTool
    {
        private static SerializerTool instance = new SerializerTool();
        public static SerializerTool Instance => instance;

        // 存储在服务器文件路径
        private string path = "D:/C#Learn/Server/HunterServer/";
        // 序列化，文件名使用类名
        public void Serialize<T>(T obj)where T:class
        {
            string filename = typeof(T).Name;

            using(FileStream fs = new FileStream(path + filename + ".cui", FileMode.OpenOrCreate, FileAccess.Write))
            {
                MessagePackSerializer.Serialize(fs, obj);
                fs.Flush();
                fs.Dispose();
            }
        }

        public T DeSerialize<T>()where T : class
        {
            string filename = path + typeof(T).Name + ".cui";
            if (!File.Exists(filename)) return default;
            T t;
            using (FileStream fs = File.Open(filename,FileMode.Open,FileAccess.Read))
            {
                t = MessagePackSerializer.Deserialize<T>(fs);
                fs.Flush();
                fs.Dispose();
            }
            return t;
        }
    }
}
