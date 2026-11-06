using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HunterGameServer
{
    // 房间中的玩家信息
    internal class PlayerData:BaseData
    {
        public string username;
        public int posX;
        public int posY;
        public bool type;   //玩家角色 true表示猎人，false表示猎物


        public override int GetByteNum()
        {
            return 4 + Encoding.UTF8.GetByteCount(username) + 4 + 4+1;
        }

        public override int Reading(byte[] bytes, int beginIndex = 0)
        {
            int index = beginIndex;
            username = ReadString(bytes, ref index);
            posX = ReadInt(bytes, ref index);
            posY = ReadInt(bytes, ref index);
            type = ReadBool(bytes, ref index);
            return index - beginIndex;
        }

        public override byte[] Writing()
        {
            byte[] bytes = new byte[GetByteNum()];
            int index = 0;
            WriteString(bytes, username, ref index);
            WriteInt(bytes, posX, ref index);
            WriteInt(bytes, posY, ref index);
            WriteBool(bytes, type, ref index);
            return bytes;
        }
    }
}
