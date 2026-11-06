using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HunterGameServer
{
    // 匹配消息
    public class MatchMsg : BaseMsg
    {
        public string username;
        public bool flag;

        public override int GetByteNum()
        {
            return 12 + 4 + Encoding.UTF8.GetByteCount(username)+1;
        }

        public override int GetTypeId()
        {
            return 1004;
        }

        public override byte[] Writing()
        {
            byte[] bytes = new byte[GetByteNum()];
            int index = 0;
            WriteInt(bytes, GetTypeId(), ref index);
            WriteInt(bytes, GetByteNum() - 8, ref index);
            WriteInt(bytes, id, ref index);
            WriteString(bytes, username, ref index);
            WriteBool(bytes,flag,ref index);
            return bytes;
        }

        public override int Reading(byte[] bytes, int beginIndex = 0)
        {
            int index = beginIndex;
            id = ReadInt(bytes,ref index);
            username = ReadString(bytes,ref index);
            flag = ReadBool(bytes,ref index);
            return index - beginIndex;
        }
    }
}
