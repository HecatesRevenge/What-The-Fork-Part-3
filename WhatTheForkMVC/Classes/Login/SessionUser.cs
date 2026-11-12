using WhatTheForkMVC.Classes.Login;

namespace WhatTheForkMVC.Classes
{
    public class SessionUser
    {
        public static void SaveUser(ISession session, UserAccounts user)
        {
            session.SetInt32("UserID", user.UserID);
            session.SetString("Username", user.Username);
            session.SetString("UserType", user.UserType);
            session.SetString("FirstName", user.FirstName);
        }

        public static UserAccounts GetUser(ISession session)
        {
            if (session.GetInt32("UserID") == null)
            {
                return null;
            }
             
            UserAccounts user = new UserAccounts();
            user.UserID = (int)session.GetInt32("UserID");
            user.Username = session.GetString("Username");
            user.UserType = session.GetString("UserType");
            user.FirstName = session.GetString("FirstName");
            return user;
        }
    }
}
