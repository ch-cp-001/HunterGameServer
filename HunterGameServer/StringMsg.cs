using System.Collections;
using System.Collections.Generic;
using System.Text;

public class StringMsg : BaseMsg
{
    public string str;

    public override int GetByteNum()
    {
        // typeid bytesNum id string
        return 16 + Encoding.UTF8.GetBytes(str).Length;

    }

    public override int GetTypeId()
    {
        return 1002;
    }

    public override int Reading(byte[] bytes, int beginIndex = 0)
    {
        int index  = beginIndex;
        id = ReadInt(bytes,ref index);
        str = ReadString(bytes,ref index);
        return index - beginIndex;
    }

    public override byte[] Writing()
    {
        byte[] bytes = new byte[GetByteNum()];
        int index = 0;
        WriteInt(bytes, GetTypeId(),ref index);
        int bytesNum = GetByteNum()-8;
        WriteInt(bytes,bytesNum, ref index);
        WriteInt(bytes,id,ref index);
        WriteString(bytes,str,ref index);
        return bytes;
    }
}
