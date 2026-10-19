 using System.Text;

public abstract class BaseData
{
    // 获取字节数组长度
    public abstract int GetByteNum();

    // 写入字节数组方法
    public abstract byte[] Writing();

    // 返回序列化的字节数组长度
    public abstract int Reading(byte[] bytes,int beginIndex=0);

    // 写入Short类型数据
    protected void WriteShort(byte[] bytes,short value,ref int index)
    {
        BitConverter.GetBytes(value).CopyTo(bytes,index);
        index += sizeof(short);
    }
    // 写入Int类型数据
    protected void WriteInt(byte[] bytes,int value,ref int index)
    {
        BitConverter.GetBytes(value).CopyTo(bytes,index);
        index += sizeof(int);
    }
    // 写入Float类型数据
    protected void WriteFloat(byte[] bytes,float value,ref int index)
    {
        BitConverter.GetBytes(value).CopyTo(bytes,index);
        index += sizeof(float);
    }
    // 写入Bool类型数据
    protected void WriteBool(byte[] bytes,bool value,ref int index)
    {
        BitConverter.GetBytes(value).CopyTo(bytes,index);
        index += sizeof(bool);
    }
    // 写入String类型数据
    protected void WriteString(byte[] bytes,string value,ref int index)
    {
        if (value == null)
        {
            WriteInt(bytes, 0, ref index);
        }
        else
        {
            byte[] strBytes = Encoding.UTF8.GetBytes(value);
            WriteInt(bytes, strBytes.Length, ref index);
            strBytes.CopyTo(bytes, index);
            index += strBytes.Length;
        }
    }
    // 写入继承BaseData类型的类的数据
    protected void WriteData(byte[] bytes, BaseData value, ref int index)
    {
        value.Writing().CopyTo(bytes, index);
        index += value.GetByteNum();
    }


    // 读取Short类型数据
    protected short ReadShort(byte[] bytes,ref int index)
    {
        short value = BitConverter.ToInt16(bytes,index);
        index += sizeof(short);
        return value;
    }
    // 读取Int类型数据
    protected int ReadInt(byte[] bytes,ref int index)
    {
        int value = BitConverter.ToInt32(bytes,index);
        index += sizeof(int);
        return value;
    }
    // 读取Float类型数据
    protected float ReadFloat(byte[] bytes,ref int index)
    {
        float value = BitConverter.ToSingle(bytes,index);
        index += sizeof(float);
        return value;
    }
    // 读取Bool类型数据
    protected bool ReadBool(byte[] bytes,ref int index)
    {
        bool value = BitConverter.ToBoolean(bytes,index);
        index += sizeof(bool);
        return value;
    }
    // 读取String类型数据
    protected string ReadString(byte[] bytes,ref int index)
    {
        int length = ReadInt(bytes,ref index);
        if (length == 0) return "";
        string value = Encoding.UTF8.GetString(bytes,index,length);
        index += length;
        return value;
    }
    // 读取继承BaseData类型的类的数据
    protected T ReadData<T>(byte[] bytes,ref int index)where T:BaseData,new()
    {
        T t = new T();
        byte[] newBytes = new byte[bytes.Length - index];
        Array.Copy(bytes, index, newBytes, 0,bytes.Length - index);
        index += t.Reading(newBytes);
        return t;
    }

}
