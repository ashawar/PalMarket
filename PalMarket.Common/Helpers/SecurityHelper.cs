using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PalMarket.Common.Helpers
{
    /// <summary>
    /// Helper class that provides security information
    /// </summary>
    public class SecurityHelper
    {
        public static string HashPassword(string password)
        {
            // Using SHA1 algorithm.
            byte[] result;
            SHA1 sha = new SHA1CryptoServiceProvider();
            result = sha.ComputeHash(Encoding.ASCII.GetBytes(password));

            StringBuilder sb = new StringBuilder();
            foreach (byte b in result)
            {
                sb.Append(b.ToString("X2"));
            }
            return sb.ToString();
        }

        public static bool VerifyHashedPassword(string HashedPassword, string Password)
        {
            string EnteredPass = HashPassword(Password);
            if (EnteredPass != HashedPassword)
            {
                return false;
            }
            return true;
        }
    }
}
