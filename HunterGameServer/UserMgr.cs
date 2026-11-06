using MessagePack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HunterGameServer
{
    [MessagePackObject]
    public class UserDic
    {
        [Key(0)] public Dictionary<string, UserData> userDic = new Dictionary<string, UserData>();
    }

    public enum UserState
    {
        Online =0,
        Maching = 1,
        Battling=2,
        Offline = 3,
    }

    // 管理用户账户信息
    //[MessagePackObject]
    public class UserMgr
    {
        private static UserMgr instance = new UserMgr();
        public static UserMgr Instance=> instance;

        // username,password
        private UserDic userInfos;

        // 所有用户的状态  账户，状态（0表示在线大厅挂机，1表示匹配中，2表示战斗中，3表示离线）
        private Dictionary<string, UserState> userStates = new Dictionary<string, UserState>();
        // 所有在线用户使用的socket
        private Dictionary<string,int> userSockets = new Dictionary<string,int>();
        // 记录所有匹配中的用户
        private List<string> matchingUsers = new List<string>();

        public UserMgr()
        {
            DeSerialize();
            if(userInfos == null) 
                userInfos = new UserDic();

            // 将所有用户状态设置为离线
            foreach (string username in userInfos.userDic.Keys)
            {
                ChangeUserState(username,UserState.Offline);
            }
        }

        public int GetUserSocket(string username)
        {
            if (userSockets.ContainsKey(username))
            {
                return userSockets[username];
            }
            return -1;
        }

        // 登录时记录用户与socket对应关系
        public void AddSocket(string username,int socketId)
        {
            if (userSockets.ContainsKey(username)) return;
            userSockets.Add(username, socketId);
        }

        // 获取玩家状态
        public UserState GetUserState(string username)
        {
            return userStates[username];
        }

        // 修改用户状态
        public void ChangeUserState(string username,UserState state)
        {
            if (userStates.ContainsKey(username))
            {
                // 如果从匹配中状态改成其他状态就从列表中移除
                if (userStates[username] == UserState.Maching && state != UserState.Maching && matchingUsers.Contains(username))
                    matchingUsers.Remove(username);
                userStates[username] = state;

                // 从其他状态变为匹配中就加入到列表中去，方便后续获取
                if (state == UserState.Maching)
                {
                    matchingUsers.Add(username);
                }
            }
            else
            {
                userStates.Add(username, state);
            }
        }
        
        // 注册时添加账户信息
        public void Add(UserData data)
        {
            if (userInfos.userDic.ContainsKey(data.userName)) return;
            data.nickName = "未命名";
            data.lev = 1;
            userInfos.userDic.Add(data.userName,data);
            Serialize();
            // 注册成功后，设置用户状态为离线
            ChangeUserState(data.userName,UserState.Offline);  
        }

        // 注册时判断账号是否存在
        public bool CheckExist(string username)
        {
            if (userInfos.userDic.ContainsKey(username)) return true;
            return false;
        }

        // 检查账户密码是否正确，并且只有离线玩家才可以登录
        public bool Check(string username,string password)
        {
            // 账号不存在
            if (!userInfos.userDic.ContainsKey(username)) return false;
            // 密码正确
            if (userStates[username] == UserState.Offline && userInfos.userDic[username].password == password) return true;
            return false;
        }

        // 提供给外部获取某一个账户信息的方法
        public UserData GetUserData(string username)
        {
            if (userInfos.userDic.ContainsKey(username))
            {
                return userInfos.userDic[username];
            }
            return null;
        }

        // 提供给外部使用的修改昵称方法
        public void ModifyNickName(string username,string nickname)
        {
            if (userInfos.userDic.ContainsKey(username))
            {
                userInfos.userDic[username].nickName = nickname;
                Serialize();
            }
        }
        // 提供给外部使用的修改密码方法
        public void ModifyPwd(string username,string pwd)
        {
            if (userInfos.userDic.ContainsKey(username))
            {
                userInfos.userDic[username].password = pwd;
                Serialize();
            }
        }

        // 外部获取匹配中用户方法
        public List<string> GetMatchUsers()
        {
            return matchingUsers;
        }

        // 序列化存储所有账户信息
        public void Serialize()
        {
            SerializerTool.Instance.Serialize(userInfos);
        }

        public void DeSerialize()
        {
            userInfos = SerializerTool.Instance.DeSerialize<UserDic>();
        }

    }
}
