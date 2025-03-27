using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace GUI.Helper
{
    public static class EncryptionHelper
    {
        private static readonly string password = "YourSecretKey123456"; // Mật khẩu để tạo khóa
        private static readonly byte[] salt = Encoding.UTF8.GetBytes("SomeSaltValue"); // Giá trị salt cố định

        public static string Encrypt(string text)
        {
            byte[] textBytes = Encoding.UTF8.GetBytes(text);

            using (Aes aes = Aes.Create())
            {
                aes.Key = GenerateKey();
                aes.IV = new byte[16]; // IV 16 byte (AES-128, AES-192, AES-256 đều dùng 16 byte IV)
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
                aes.IV = new byte[16]; // Phải giống với Encrypt()
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
                return deriveBytes.GetBytes(32); // Lấy 32 bytes cho AES-256
            }
        }
    }
}
