using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HunterGameServer
{
    // 管理用户账户信息
    internal class UserMgr
    {
        private static UserMgr instance = new UserMgr();
        public static UserMgr Instance=> instance;
        
        // username,password
        private Dictionary<string,UserData> userDic = new Dictionary<string, UserData>();

        // 添加账户信息
        public void Add(UserData data)
        {
            if (userDic.ContainsKey(data.userName)) return;
            data.nickName = "未命名";
            data.lev = 1;
            userDic.Add(data.userName,data);
        }

        // 注册时判断账号是否存在
        public bool CheckExist(string username)
        {
            if (userDic.ContainsKey(username)) return true;
            return false;
        }

        // 检查账户密码是否正确
        public bool Check(string username,string password)
        {
            // 账号不存在
            if (!userDic.ContainsKey(username)) return false;
            // 密码正确
            if (userDic[username].password == password) return true;
            return false;
        }

        // 提供给外部获取某一个账户信息的方法
        public UserData GetUserData(string username)
        {
            if (userDic.ContainsKey(username))
            {
                return userDic[username];
            }
            return null;
        }

        // 提供给外部使用的修改昵称方法
        public void ModifyNickName(string username,string nickname)
        {
            if(userDic.ContainsKey(username))
                userDic[username].nickName = nickname;
        }
        // 提供给外部使用的修改密码方法
        public void ModifyPwd(string username,string pwd)
        {
            if(userDic.ContainsKey(username))
                userDic[username].password = pwd;
        }
    }
}
