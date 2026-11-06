using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HunterGameServer
{
    internal class TerrainMsg:BaseMsg
    {
        public TerrainData data;

        public override int GetByteNum()
        {
            return 12 + data.GetByteNum();
        }

        public override int GetTypeId()
        {
            return 1006;
        }

        public override int Reading(byte[] bytes, int beginIndex = 0)
        {
            int index = beginIndex;
            id = ReadInt(bytes, ref index);
            data = ReadData<TerrainData>(bytes, ref index);
            return index - beginIndex;
        }

        public override byte[] Writing()
        {
            byte[] bytes = new byte[GetByteNum()];
            int index = 0;
            WriteInt(bytes, GetTypeId(), ref index);
            WriteInt(bytes, GetByteNum() - 8, ref index);
            WriteInt(bytes,id,ref index);
            WriteData(bytes, data, ref index);
            return bytes;
        }
    }
}
