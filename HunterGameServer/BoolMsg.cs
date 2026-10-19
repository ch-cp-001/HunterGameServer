public class BoolMsg : BaseMsg
{
    public bool flag;

    // ÏûÏ¢Í·£¨typeid+bytenum£©+id+flag
    public override int GetByteNum()
    {
        return 4 + 4 + 4 + 1;
    }

    public override int GetTypeId()
    {
        return 1003;
    }

    public override int Reading(byte[] bytes, int beginIndex = 0)
    {
        int index = beginIndex;
        id = ReadInt(bytes,ref index);
        flag = ReadBool(bytes,ref index);
        return index - beginIndex;
    }

    public override byte[] Writing()
    {
        byte[] bytes = new byte[GetByteNum()];
        int index = 0;
        WriteInt(bytes,GetTypeId(),ref index);  // typeid
        WriteInt(bytes,GetByteNum()-8,ref index);   // bytenum
        WriteInt(bytes,id,ref index);   //id
        WriteBool(bytes,flag,ref index);    // flag
        return bytes;
    }
}
