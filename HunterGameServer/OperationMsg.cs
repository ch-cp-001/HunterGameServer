
// 移动消息，告知服务端需要移动，服务端接收后需要返回可移动范围
public class OperationMsg : BaseMsg
{
    // 房间号，身份（猎人或猎物）
    public int roomId;
    public bool type;
    public int operation;   // 操作类型  0：移动，1：休息

    public override int GetByteNum()
    {
        return 21;
    }

    public override int GetTypeId()
    {
        return 1009;
    }

    public override int Reading(byte[] bytes, int beginIndex = 0)
    {
        int index = beginIndex;
        id = ReadInt(bytes, ref index);
        roomId = ReadInt(bytes, ref index);
        type = ReadBool(bytes, ref index);
        operation = ReadInt(bytes, ref index);
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
        WriteInt(bytes, operation, ref index);
        return bytes;
    }
}
