public class BaseMsg : BaseData
{
    // 消息id
    public int id;

    // 数据体部分长度
    public override int GetByteNum()
    {
        throw new System.NotImplementedException();
    }

    public override int Reading(byte[] bytes, int beginIndex = 0)
    {
        throw new System.NotImplementedException();
    }

    /// <summary>
    /// 实现序列化为字计数组方法是需要先序列化类型id
    /// </summary>
    /// <returns></returns>
    /// <exception cref="System.NotImplementedException"></exception>
    public override byte[] Writing()
    {
        throw new System.NotImplementedException();
    }

    // 获取当前类对应的编号，用于再传输数据后区分类型
    public virtual int GetTypeId()
    {
        return 0;
    }
}
