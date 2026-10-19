using HunterGameServer;

public abstract class BaseHandler
{
    public BaseMsg message;

    // 将使用的Socket作为参数传入，便于后续有逻辑需要处理后再向客户端发送
    public abstract void Handle(ClientSocket socket);
}
