
using HunterGameServer;

public class MsgPool
{
	private static MsgPool instance = new MsgPool();
	public static MsgPool Instance => instance;

	public Dictionary<int,Type> msgDic = new Dictionary<int,Type>();
	public Dictionary<int,Type> handleDic = new Dictionary<int,Type>();
	
	public MsgPool()
	{
		Register(1001,typeof(UserMsg),typeof(UserMsgHandler));
		Register(1003,typeof(BoolMsg),typeof(BoolMsgHandler));
	}
	public void Register(int id,Type baseMsg,Type baseHandler)
	{
		msgDic.Add(id,baseMsg);
		handleDic.Add(id,baseHandler);
	}
	public BaseMsg GetMsg(int id)
	{
		if (!msgDic.ContainsKey(id))
		{
			return null;
		}
		return Activator.CreateInstance(msgDic[id]) as BaseMsg;
	}
	public BaseHandler GetHandler(int id)
	{
		if (!handleDic.ContainsKey(id))
		{
			return null;
		}
		return Activator.CreateInstance(handleDic[id]) as BaseHandler;
		}
	}
