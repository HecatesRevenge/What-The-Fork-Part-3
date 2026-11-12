using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace WhatTheForkMVC.Classes.Security
{
    public class PasswordHasher
    {
        public string HashPassword(String plainTextPassword)
        {
            return BCrypt.Net.BCrypt.HashPassword(plainTextPassword);
        }

        public bool VerifyPassword(String plainTextPassword, String hashedPassword)
        {


            try
            {
                return BCrypt.Net.BCrypt.Verify(plainTextPassword, hashedPassword);

            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}