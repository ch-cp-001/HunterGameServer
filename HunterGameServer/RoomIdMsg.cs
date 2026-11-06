using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HunterGameServer
{
    internal class RoomIdMsg : BaseMsg
    {
        public int roomId;

        public override int GetByteNum()
        {
            return 16;
        }

        public override int GetTypeId()
        {
            return 1011;
        }

        public override int Reading(byte[] bytes, int beginIndex = 0)
        {
            int index = beginIndex;
            id = ReadInt(bytes, ref index);
            roomId = ReadInt(bytes, ref index);
            return index - beginIndex;
        }

        public override byte[] Writing()
        {
            byte[] bytes = new byte[GetByteNum()];
            int index = 0;
            WriteInt(bytes, GetTypeId(), ref index);
            WriteInt(bytes, GetByteNum() - 8, ref index);
            WriteInt(bytes, id, ref index);
            WriteInt(bytes,roomId,ref index);
            return bytes;
        }
    }
}
