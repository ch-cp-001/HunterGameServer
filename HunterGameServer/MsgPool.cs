
using HunterGameServer;

public class MsgPool
{
	private static MsgPool instance = new MsgPool();
	public static MsgPool Instance => instance;

	public Dictionary<int,Type> msgDic = new Dictionary<int,Type>();
	public Dictionary<int,Type> handleDic = new Dictionary<int,Type>();
	
	public MsgPool()
	{
		Register(999,typeof(HeartMsg),typeof(HeartMsgHandler));
		Register(1001,typeof(UserMsg),typeof(UserMsgHandler));
		Register(1003,typeof(BoolMsg),typeof(BoolMsgHandler));
		Register(1004,typeof(MatchMsg),typeof(MatchMsgHandler));
		// 1005 playermsg
		// 1006 terrainmsg
		// 1007 enemyname
		Register(1008,typeof(UserQuitMsg),typeof(UserQuitMsgHandler));
		Register(1009,typeof(MoveMsg), typeof(MoveMsgHandler));

		Register(1010,typeof(QuitMsg),typeof(QuitMsgHandler));
		Register(1013,typeof(MovePosMsg),typeof(MovePosMsgHandler));
		//Register(1014,typeof(MovePathMsg),typeof(MovePathMsgHandler));

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
