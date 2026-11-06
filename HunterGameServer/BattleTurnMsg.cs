using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HunterGameServer
{
    internal class BattleTurnMsg:BaseMsg
    {
        public int turn;
        public int maxTurn;

        public override int GetByteNum()
        {
            return 20;
        }

        public override int GetTypeId()
        {
            return 1016;
        }

        public override int Reading(byte[] bytes, int beginIndex = 0)
        {
            int index = beginIndex;
            id = ReadInt(bytes, ref index);
            turn = ReadInt(bytes, ref index);
            maxTurn = ReadInt(bytes, ref index);
            return index - beginIndex;
        }

        public override byte[] Writing()
        {
            byte[] bytes = new byte[GetByteNum()];
            int index = 0;
            WriteInt(bytes, GetTypeId(), ref index);
            WriteInt(bytes, GetByteNum() - 8, ref index);
            WriteInt(bytes, id, ref index);
            WriteInt(bytes,turn,ref index);
            WriteInt(bytes,maxTurn,ref index);
            return bytes;
        }
    }
}
