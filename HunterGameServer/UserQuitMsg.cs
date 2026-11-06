
// 用户退出消息给服务端，修改用户在线状态
using System.Text;

public class UserQuitMsg : BaseMsg
{
    public string username;

    public override int GetByteNum()
    {
        return 12 + 4 + Encoding.UTF8.GetByteCount(username);
    }

    public override int GetTypeId()
    {
        return 1008;
    }

    public override byte[] Writing()
    {
        byte[] bytes = new byte[GetByteNum()];
        int index = 0;
        WriteInt(bytes, GetTypeId(), ref index);
        WriteInt(bytes, 0, ref index);
        WriteInt(bytes, id, ref index);
        WriteString(bytes, username, ref index);
        return bytes;
    }

    public override int Reading(byte[] bytes, int beginIndex = 0)
    {
        int index = beginIndex;
        id = ReadInt(bytes,ref index);
        username = ReadString(bytes,ref index);
        return index - beginIndex;
    }
}