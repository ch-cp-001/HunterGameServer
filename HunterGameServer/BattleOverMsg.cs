using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HunterGameServer
{
    // 战斗结果信息类
    internal class BattleOverMsg : BaseMsg
    {
        public bool res;    // true表示胜利，false表示失败

        public override int GetByteNum()
        {
            return 13;
        }

        public override int GetTypeId()
        {
            return 1017;
        }

        public override int Reading(byte[] bytes, int beginIndex = 0)
        {
            int index = beginIndex;
            id = ReadInt(bytes, ref index);
            res = ReadBool(bytes,ref index);
            return index - beginIndex;
        }

        public override byte[] Writing()
        {
            byte[] bytes = new byte[GetByteNum()];
            int index = 0;
            WriteInt(bytes, GetTypeId(), ref index);
            WriteInt(bytes, GetByteNum() - 8, ref index);
            WriteInt(bytes, id, ref index);
            WriteBool(bytes,res,ref index);
            return bytes;
        }
    }
}
