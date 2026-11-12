using System;
namespace WhatTheForkMVC.Classes
{
    public class LoginCookie
    {
        EncryptionHelper objEncryption = new EncryptionHelper();
        private String cookieName = "WTF_LoginID";

        public void SaveLoginID(HttpResponse response, string username)
        {
            CookieOptions options = new CookieOptions();
            options.Expires = DateTime.Now.AddDays(60);
            response.Cookies.Append(cookieName, objEncryption.Encrypt(username), options);
        }

        public String ReadLoginID(HttpRequest request)
        {
            if (request.Cookies[cookieName] != null)
            {
                return objEncryption.Decrypt(request.Cookies[cookieName]);
            }
            return "";

        }
        public void DeleteLoginID(HttpResponse response)
        {
            response.Cookies.Delete(cookieName);
        }


    }
}
