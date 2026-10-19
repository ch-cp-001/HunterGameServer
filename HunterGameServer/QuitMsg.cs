
public class QuitMsg : BaseMsg
{
    // 这个时数据体部分的长度
    public override int GetByteNum()
    {
        return 8;
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
        return bytes;
    }
}
