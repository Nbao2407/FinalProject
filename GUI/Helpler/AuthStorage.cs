using System;
using System.IO;
using GUI.Helper;
using Newtonsoft.Json;

namespace GUI
{
    public class AuthData
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public static class AuthStorage
    {
        private static readonly string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "auth.json");

        public static void SaveCredentials(string username, string email, string password)
        {
            var data = new AuthData
            {
                Username = username,
                Email = email,
                Password = EncryptionHelper.Encrypt(password)
            };

            File.WriteAllText(filePath, JsonConvert.SerializeObject(data));
        }

        public static AuthData LoadCredentials()
        {
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                var data = JsonConvert.DeserializeObject<AuthData>(json);

                if (data != null)
                {
                    data.Password = EncryptionHelper.Decrypt(data.Password);
                    return data;
                }
            }
            return null;
        }

        public static void ClearCredentials()
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }
}
