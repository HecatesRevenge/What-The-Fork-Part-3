using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace WhatTheForkMVC.Classes
{
    public class EncryptionHelper
    {
        private Byte[] key;

        public EncryptionHelper()
        {
            IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();

            key = Convert.FromBase64String(config["Encryption:Key"]);
        }

        public String Encrypt(String plainText)
        {
            if (String.IsNullOrEmpty(plainText))
            {
                return "";
            }

            UTF8Encoding encoder = new UTF8Encoding();
            Byte[] textBytes = encoder.GetBytes(plainText);

            Aes aesEncryption = Aes.Create();
            aesEncryption.Key = key;
            aesEncryption.GenerateIV();
            Byte[] vector = aesEncryption.IV;

            MemoryStream myMemoryStream = new MemoryStream();
            myMemoryStream.Write(vector, 0, vector.Length);

            CryptoStream myEncryptionStream = new CryptoStream(myMemoryStream, aesEncryption.CreateEncryptor(), CryptoStreamMode.Write);
            myEncryptionStream.Write(textBytes, 0, textBytes.Length);
            myEncryptionStream.FlushFinalBlock();

            Byte[] encryptedBytes = myMemoryStream.ToArray();

            myEncryptionStream.Close();
            myMemoryStream.Close();

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
                Byte[] allBytes = Convert.FromBase64String(encryptedText);
                if (allBytes.Length <= 16)
                {
                    return encryptedText;
                }

                UTF8Encoding encoder = new UTF8Encoding();

                Aes aesEncryption = Aes.Create();
                aesEncryption.Key = key;

                Byte[] vector = new Byte[16];
                Array.Copy(allBytes, 0, vector, 0, 16);
                aesEncryption.IV = vector;

                MemoryStream myMemoryStream = new MemoryStream();
                CryptoStream myDecryptionStream = new CryptoStream(myMemoryStream, aesEncryption.CreateDecryptor(), CryptoStreamMode.Write);

                myDecryptionStream.Write(allBytes, 16, allBytes.Length - 16);
                myDecryptionStream.FlushFinalBlock();

                Byte[] textBytes = myMemoryStream.ToArray();

                myDecryptionStream.Close();
                myMemoryStream.Close();

                return encoder.GetString(textBytes);
            }
            catch (Exception ex)
            {
                return encryptedText;
            }
        }
    }
}
