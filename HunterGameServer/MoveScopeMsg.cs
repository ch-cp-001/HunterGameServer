
namespace HunterGameServer
{
    internal class MoveScopeMsg : BaseMsg
    {
        public List<int> moves;

        public override int GetByteNum()
        {
            return 12+4+moves.Count*4;
        }

        public override int GetTypeId()
        {
            return 1012;
        }

        public override int Reading(byte[] bytes, int beginIndex = 0)
        {
            int index = beginIndex;
            id = ReadInt(bytes,ref index);
            int count = ReadInt(bytes,ref index);
            if (count > 0)
            {
                moves = new List<int>();
                for(int i = 0; i < count; i++)
                {
                    moves.Add(ReadInt(bytes,ref index));
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
            if(moves != null)
            {
                WriteInt(bytes, moves.Count, ref index);
                if(moves.Count > 0)
                {
                    for(int i = 0; i < moves.Count; i++)
                    {
                        WriteInt(bytes, moves[i], ref index);
                    }
                }
            }

            return bytes;
        }
    }
}
