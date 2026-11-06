// 断开socket
public class QuitMsg : BaseMsg
{
    // 这个时数据体部分的长度
    public override int GetByteNum()
    {
        return 12;
    }

    public override int GetTypeId()
    {
        return 1010;
    }

    public override byte[] Writing()
    {
        byte[] bytes = new byte[GetByteNum()];
        int index = 0;
        WriteInt(bytes,GetTypeId(),ref index);
        WriteInt(bytes,GetByteNum()-8,ref index);
        WriteInt(bytes,id,ref index);
        return bytes;
    }

    public override int Reading(byte[] bytes, int beginIndex = 0)
    {
        int index = beginIndex;
        id = ReadInt(bytes, ref index);
        return index - beginIndex;
    }
}
