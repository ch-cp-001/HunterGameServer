
namespace HunterGameServer
{
    internal class MovePathMsg : BaseMsg
    {
        public List<int> path;

        public override int GetByteNum()
        {
            return 12+4+path.Count*4;
        }

        public override int GetTypeId()
        {
            return 1014;
        }

        public override int Reading(byte[] bytes, int beginIndex = 0)
        {
            int index = beginIndex;
            id = ReadInt(bytes,ref index);
            int count = ReadInt(bytes,ref index);
            if (count > 0)
            {
                path = new List<int>();
                for(int i = 0; i < count; i++)
                {
                    path.Add(ReadInt(bytes,ref index));
                }
            }
            return index - beginIndex;
        }

        public override byte[] Writing()
        {
            byte[] bytes = new byte[GetByteNum()];
            int index = 0;
            WriteInt(bytes,GetTypeId(),ref index);
            WriteInt(bytes,GetByteNum()-8,ref index);
            WriteInt(bytes,id,ref index);
            if(path != null)
            {
                WriteInt(bytes, path.Count, ref index);
                if(path.Count > 0)
                {
                    for(int i = 0; i < path.Count; i++)
                    {
                        WriteInt(bytes, path[i], ref index);
                    }
                }
            }

            return bytes;
        }
    }
}
