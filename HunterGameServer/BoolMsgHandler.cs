
using HunterGameServer;

public class BoolMsgHandler : BaseHandler
{
    public override void Handle(ClientSocket socket)
    {
        bool flag = (message as BoolMsg).flag;
        Console.WriteLine(flag);
    }
}
