
namespace HunterGameServer
{
    internal class UserQuitMsgHandler : BaseHandler
    {
        public override void Handle(ClientSocket socket)
        {
            UserQuitMsg msg = message as UserQuitMsg;
            if (msg != null)
            {
                UserMgr.Instance.ChangeUserState(msg.username,UserState.Offline);
            }
        }
    }
}
