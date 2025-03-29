using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace GUI.Helper
{
    public static class EncryptionHelper
    {
        private static readonly string password = "YourSecretKey123456"; 
        private static readonly byte[] salt = Encoding.UTF8.GetBytes("SomeSaltValue"); 

        public static string Encrypt(string text)
        {
            byte[] textBytes = Encoding.UTF8.GetBytes(text);

            using (Aes aes = Aes.Create())
            {
                aes.Key = GenerateKey();
                aes.IV = new byte[16]; 
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                using (ICryptoTransform encryptor = aes.CreateEncryptor())
                {
                    byte[] encryptedBytes = encryptor.TransformFinalBlock(textBytes, 0, textBytes.Length);
                    return Convert.ToBase64String(encryptedBytes);
                }
            }
        }

        public static string Decrypt(string encryptedText)
        {
            byte[] encryptedBytes = Convert.FromBase64String(encryptedText);

            using (Aes aes = Aes.Create())
            {
                aes.Key = GenerateKey();
                aes.IV = new byte[16]; 
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                using (ICryptoTransform decryptor = aes.CreateDecryptor())
                {
                    byte[] decryptedBytes = decryptor.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);
                    return Encoding.UTF8.GetString(decryptedBytes);
                }
            }
        }

        private static byte[] GenerateKey()
        {
            using (var deriveBytes = new Rfc2898DeriveBytes(password, salt, 10000))
            {
                return deriveBytes.GetBytes(32); 
            }
        }
    }
}
