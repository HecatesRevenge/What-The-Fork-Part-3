using System;
using System.Security.Cryptography;
using System.Text;
namespace WhatTheForkMVC.Classes
{
    public class EncryptionHelper
    {
        private Byte[] key = { 166, 229, 229, 158, 163, 41, 249, 102, 5, 56, 252, 26, 48, 59, 30, 2 };
        private Byte[] vector = { 117, 163, 99, 156, 140, 191, 38, 125, 212, 169, 164, 23, 75, 253, 226, 80 };

        public String Encrypt(String plainText)
        {
            if (String.IsNullOrEmpty(plainText))
            {
                return "";

            }

            UTF8Encoding encoder = new UTF8Encoding();
            Byte[] textBytes = encoder.GetBytes(plainText);

            Aes aesEncryption = Aes.Create();
            MemoryStream thisMemoryStream = new MemoryStream();
            CryptoStream thisEncryptionStream = new CryptoStream(thisMemoryStream, aesEncryption.CreateEncryptor(key, vector), CryptoStreamMode.Write);

            thisEncryptionStream.Write(textBytes, 0, textBytes.Length);
            thisEncryptionStream.FlushFinalBlock();

            thisMemoryStream.Position = 0;
            Byte[] encryptedBytes=new byte[thisMemoryStream.Length];
            thisMemoryStream.Read(encryptedBytes, 0, encryptedBytes.Length);

            thisEncryptionStream.Close();
            thisMemoryStream.Close();

            return Convert.ToBase64String(encryptedBytes);

        }

        public String Decrypt(String encryptedText)
        {

            if (String.IsNullOrEmpty(encryptedText))
            {
                return "";
            }

            try
            {
                Byte[] encryptedBytes = Convert.FromBase64String(encryptedText);
                UTF8Encoding encoder=new UTF8Encoding();

                Aes aesEncrypt=Aes.Create();
                MemoryStream thisMemoryStream = new MemoryStream();
                CryptoStream thisDecryptionStream=new CryptoStream(thisMemoryStream, aesEncrypt.CreateDecryptor(key, vector), CryptoStreamMode.Write);

                thisDecryptionStream.Write(encryptedBytes, 0, encryptedBytes.Length);
                thisDecryptionStream.FlushFinalBlock();

                thisMemoryStream.Position = 0;
                Byte[] textBytes = new Byte[thisMemoryStream.Length];
                thisMemoryStream.Read(textBytes, 0, textBytes.Length);

                thisDecryptionStream.Close();
                thisMemoryStream.Close();

                return encoder.GetString(textBytes);
            }
            catch (Exception ex)
            {
                return encryptedText;
            }

          
        }
    }
}
