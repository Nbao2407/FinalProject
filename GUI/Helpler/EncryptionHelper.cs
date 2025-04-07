using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace QLVT.Helper
{
    public static class EncryptionHelper
    {
        private static readonly string password = " SecretKey123456";
        private static readonly byte[] salt = Encoding.UTF8.GetBytes("adminCarePls");

        public static string Encrypt(string text)
        {
            byte[] textBytes = Encoding.UTF8.GetBytes(text);

            using (Aes aes = Aes.Create())
            {
                aes.Key = GenerateKey();
                aes.GenerateIV(); 
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                using (ICryptoTransform encryptor = aes.CreateEncryptor())
                {
                    byte[] encryptedBytes = encryptor.TransformFinalBlock(textBytes, 0, textBytes.Length);
                    byte[] result = new byte[aes.IV.Length + encryptedBytes.Length];
                    Buffer.BlockCopy(aes.IV, 0, result, 0, aes.IV.Length);
                    Buffer.BlockCopy(encryptedBytes, 0, result, aes.IV.Length, encryptedBytes.Length);
                    return Convert.ToBase64String(result);
                }
            }
        }

        public static string Decrypt(string encryptedText)
        {
            byte[] fullBytes = Convert.FromBase64String(encryptedText);

            using (Aes aes = Aes.Create())
            {
                aes.Key = GenerateKey();
                byte[] iv = new byte[16];
                Buffer.BlockCopy(fullBytes, 0, iv, 0, iv.Length);
                aes.IV = iv;
                byte[] encryptedBytes = new byte[fullBytes.Length - iv.Length];
                Buffer.BlockCopy(fullBytes, iv.Length, encryptedBytes, 0, encryptedBytes.Length);

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