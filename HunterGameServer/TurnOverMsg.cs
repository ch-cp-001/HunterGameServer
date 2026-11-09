

// 移动端动画等效果一回合结束后
public class TurnOverMsg : BaseMsg
{
    // 房间号，身份（猎人或猎物）
    public int roomId;

    public override int GetByteNum()
    {
        return 16;
    }

    public override int GetTypeId()
    {
        return 1017;
    }

    public override int Reading(byte[] bytes, int beginIndex = 0)
    {
        int index = beginIndex;
        id = ReadInt(bytes, ref index);
        roomId = ReadInt(bytes, ref index);
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
        return bytes;
    }
}
