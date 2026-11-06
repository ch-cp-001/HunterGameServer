
public class MovePosMsg : BaseMsg
{
    // 房间号，身份（猎人或猎物）
    public int roomId;
    public bool type;
    // 目标位置
    public int x;
    public int y;

    public override int GetByteNum()
    {
        return 25; // 12+4+1+4+4
    }

    public override int GetTypeId()
    {
        return 1013;
    }

    public override int Reading(byte[] bytes, int beginIndex = 0)
    {
        int index = beginIndex;
        id = ReadInt(bytes,ref index);
        roomId = ReadInt(bytes,ref index);
        type = ReadBool(bytes,ref index);
        x = ReadInt(bytes,ref index);
        y = ReadInt(bytes,ref index);
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
        WriteInt(bytes, x, ref index);
        WriteInt(bytes, y, ref index);
        return bytes;
    }
}
