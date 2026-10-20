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

    // 管理用户账户信息
    //[MessagePackObject]
    public class UserMgr
    {
        private static UserMgr instance = new UserMgr();
        public static UserMgr Instance=> instance;

        // username,password
        public UserDic userInfos;
        //[Key(0)] public Dictionary<string,UserData> userDic = new Dictionary<string, UserData>();

        public UserMgr()
        {
            DeSerialize();
            if(userInfos == null) 
                userInfos = new UserDic();
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
        
        // 添加账户信息
        public void Add(UserData data)
        {
            if (userInfos.userDic.ContainsKey(data.userName)) return;
            data.nickName = "未命名";
            data.lev = 1;
            userInfos.userDic.Add(data.userName,data);
            Serialize();
        }

        // 注册时判断账号是否存在
        public bool CheckExist(string username)
        {
            if (userInfos.userDic.ContainsKey(username)) return true;
            return false;
        }

        // 检查账户密码是否正确
        public bool Check(string username,string password)
        {
            // 账号不存在
            if (!userInfos.userDic.ContainsKey(username)) return false;
            // 密码正确
            if (userInfos.userDic[username].password == password) return true;
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
            if(userInfos.userDic.ContainsKey(username))
                userInfos.userDic[username].nickName = nickname;
        }
        // 提供给外部使用的修改密码方法
        public void ModifyPwd(string username,string pwd)
        {
            if(userInfos.userDic.ContainsKey(username))
                userInfos.userDic[username].password = pwd;
        }
    }
}
