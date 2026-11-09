using System;
using WhatTheForkMVC.Models;
using WhatTheForkMVC.Classes;
using System.Data;
namespace WhatTheForkMVC.Classes
{
    public class SecurityQuestionChecks
    {
        AccountDB odjAccountDB = new AccountDB();

        //TODO: Write my own password hasher class to hash the security question answers. 
        PasswordHasher odjHash =new PasswordHasher();

        public String CleanResponse( String answer)
        {
            if (answer == null)
            {
                return null;
            }
            return answer.Trim().ToLower();
        }

        public DataRow SelectRandomnSecurityQuestion(int userID)
        {
            DataSet ds=odjAccountDB.GetUserSecurityQuestions(userID);

            if (ds.Tables[0].Rows.Count == 0)
            {
                return null;
            }
            
            Random rand = new Random();

            int index=rand.Next(0, ds.Tables[0].Rows.Count);
            return ds.Tables[0].Rows[index];
        }

        public bool CheckAnswer(int userID, int questionID, String answer)
        {
            DataSet ds = odjAccountDB.GetUserSecurityQuestions(userID);

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                if (Convert.ToInt32(row["QuestionID"]) == questionID)
                {
                    String hashedAnswer = row["AnswerHash"].ToString();
                    return odjHash.VerifyPassword(CleanResponse(answer), hashedAnswer);
                }
            }
            return false;
        }
    }
}
