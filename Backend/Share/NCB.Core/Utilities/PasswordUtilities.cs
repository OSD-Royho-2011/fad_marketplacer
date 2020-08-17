using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace NCB.Core.Utilities
{
    public static class PasswordUtilities
    {
        public static void GeneratePassword(this string password, out string passwordSalt, out string passwordHash)
        {
            passwordSalt = GetPasswordSalt();
            passwordHash = CreateHashPassword(password, passwordSalt);
        }

        public static bool VerifyPassword(string pass, string saltKey, string passHash)
        {
            var comparePass = CreateHashPassword(pass, saltKey);
            return passHash.Equals(comparePass);
        }
        private static string GetRandomString(int lenght = 8)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            return new string(
                Enumerable.Repeat(chars, lenght)
                          .Select(s => s[random.Next(s.Length)])
                          .ToArray());
        }

        private static string GetPasswordSalt()
        {
            string saltKey = GetRandomString();
            return saltKey;
        }

        private static string CreateHashPassword(string pass, string saltKey)
        {
            var md5 = new MD5CryptoServiceProvider();
            var passHash = md5.ComputeHash(Encoding.UTF8.GetBytes(string.Format($"{saltKey}{pass}")));
            return Convert.ToBase64String(passHash, 0, passHash.Length);
        }
    }
}
