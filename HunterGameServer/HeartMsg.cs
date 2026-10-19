

public class HeartMsg : BaseMsg
{
    public override int GetByteNum()
    {
        return 8;
    }

    public override int GetTypeId()
    {
        return 999;
    }

    public override byte[] Writing()
    {
        byte[] bytes = new byte[GetByteNum()];
        int index = 0;
        WriteInt(bytes,GetTypeId(),ref index);
        WriteInt(bytes,0,ref index);
        return bytes;
    }
}
