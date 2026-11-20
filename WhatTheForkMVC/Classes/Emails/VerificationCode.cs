namespace WhatTheForkMVC.Classes.Emails
{
    public class VerificationCode
    {
        //Make more complicated later if needed.
        public static String Generate()
        {
            Random random = new Random();
            int code = random.Next(100000, 1000000);
            return code.ToString();
        }
    }
}

