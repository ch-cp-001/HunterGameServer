// 没有具体数据的消息
public class NormalMsg : BaseMsg
{
    private int typeId;

    public override int GetByteNum()
    {
        return 8;
    }

    public override int GetTypeId()
    {
        return typeId;
    }

    public override byte[] Writing()
    {
        byte[] bytes = new byte[GetByteNum()];
        int index = 0;
        WriteInt(bytes, GetTypeId(), ref index);
        WriteInt(bytes, 0, ref index);
        return bytes;
    }

    // 修改typeId
    public void ModifyTypeId(int id)
    {
        // 1004-Match开始匹配消息
        typeId = id;
    }
}
