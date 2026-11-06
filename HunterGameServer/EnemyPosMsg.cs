using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HunterGameServer
{
    // 发送敌人移动位置信息给客户端
    internal class EnemyPosMsg:BaseMsg
    {
        // 目标位置
        public int x;
        public int y;

        public override int GetByteNum()
        {
            return 20; // 12+4+4
        }

        public override int GetTypeId()
        {
            return 1015;
        }

        public override int Reading(byte[] bytes, int beginIndex = 0)
        {
            int index = beginIndex;
            id = ReadInt(bytes, ref index);
            x = ReadInt(bytes, ref index);
            y = ReadInt(bytes, ref index);
            return index - beginIndex;
        }

        public override byte[] Writing()
        {
            byte[] bytes = new byte[GetByteNum()];
            int index = 0;
            WriteInt(bytes, GetTypeId(), ref index);
            WriteInt(bytes, GetByteNum() - 8, ref index);
            WriteInt(bytes, id, ref index);
            WriteInt(bytes, x, ref index);
            WriteInt(bytes, y, ref index);
            return bytes;
        }
    }
}
