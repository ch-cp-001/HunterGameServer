
namespace HunterGameServer
{

    public class UserMsg : BaseMsg
    {
        public UserData data;

        // 消息长度  typeid+bytenum+id+data
        public override int GetByteNum()
        {
            return 8 + 4 + data.GetByteNum();
        }

        public override int GetTypeId()
        {
            return 1001;
        }

        // 不需要反序列化
        public override int Reading(byte[] bytes, int beginIndex = 0)
        {
            int index = beginIndex;
            id = ReadInt(bytes, ref index);
            data = ReadData<UserData>(bytes, ref index);
            return index - beginIndex;
        }

        // 序列化  注意要加上typeid（区分消息类型）和bytenum（解决分包粘包）
        public override byte[] Writing()
        {
            byte[] bytes = new byte[GetByteNum()];
            int index = 0;
            WriteInt(bytes, GetTypeId(), ref index);
            WriteInt(bytes, GetByteNum() - 8, ref index);
            WriteInt(bytes, id, ref index);
            WriteData(bytes, data, ref index);

            return bytes;
        }
    }
}
