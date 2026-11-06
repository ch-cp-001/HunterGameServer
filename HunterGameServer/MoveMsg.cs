
// 移动消息，告知服务端需要移动，服务端接收后需要返回可移动范围
public class MoveMsg : BaseMsg
{
    // 房间号，身份（猎人或猎物）
    public int roomId;
    public bool type;

    public override int GetByteNum()
    {
        return 12;
    }

    public override int GetTypeId()
    {
        return 1009;
    }

    public override int Reading(byte[] bytes, int beginIndex = 0)
    {
        int index = beginIndex;
        roomId = ReadInt(bytes, ref index);
        type = ReadBool(bytes, ref index);
        id = ReadInt(bytes, ref index);
        return index - beginIndex;
    }

    public override byte[] Writing()
    {
        byte[] bytes = new byte[GetByteNum()];
        int index = 0;
        WriteInt(bytes, GetTypeId(), ref index);
        WriteInt(bytes, GetByteNum() - 8, ref index);
        WriteInt(bytes, id, ref index);
        WriteInt(bytes, roomId, ref index);
        WriteBool(bytes, type, ref index);
        return bytes;
    }
}
