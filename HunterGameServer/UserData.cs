using MessagePack;
using System.Text;

namespace HunterGameServer
{
    [MessagePackObject]
    public class UserData : BaseData
    {
        // 标识 (0表示客户端注册，1表示客户端登录，2客户端修改昵称，3客户端修改密码，4服务端发送新账户数据给客户端)
        [Key(0)] public short flag;
        // 用户账号
        [Key(1)] public string userName="";
        // 用户密码
        [Key(2)] public string password="";
        // 昵称
        [Key(3)] public string nickName="";
        // 等级
        [Key(4)] public int lev;

        // 获取字节数组长度  字符串字节长度+字符串
        public override int GetByteNum()
        {
            // flag,username,password,nickname,lev
            int length = 2+4 + Encoding.UTF8.GetByteCount(userName) + 4 + Encoding.UTF8.GetByteCount(password)
                + 4 + Encoding.UTF8.GetByteCount(nickName) + 4;
            return length;
        }

        // 序列化
        public override byte[] Writing()
        {
            byte[] bytes = new byte[GetByteNum()];
            int index = 0;
            WriteShort(bytes, flag, ref index);
            WriteString(bytes, userName, ref index);
            WriteString(bytes, password, ref index);
            WriteString(bytes, nickName, ref index);
            WriteInt(bytes, lev, ref index);
            return bytes;
        }

        // 不需要反序列化
        public override int Reading(byte[] bytes, int beginIndex = 0)
        {
            int index = beginIndex;
            flag = ReadShort(bytes, ref index);
            userName = ReadString(bytes, ref index);
            password = ReadString(bytes, ref index);
            nickName = ReadString(bytes, ref index);
            lev = ReadInt(bytes, ref index);
            return index - beginIndex;
        }
    }

}
