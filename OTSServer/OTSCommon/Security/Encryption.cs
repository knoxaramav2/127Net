using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace OTSCommon.Security
{
    public static class Reversible
    {

    }

    public static class Irreversible
    {
        public static string GenerateSalt()
        {
            const int saltLength = 32;
            var salt = new byte[saltLength];
            using(var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            return salt.ToString()!;
        }

        public static string CreateSaltedHash(string password, string? salt=null)
        {
            salt ??= GenerateSalt();
            var cmb = password + salt;
            var hash = new StringBuilder(cmb);
            var hashArray = SHA256.HashData(Encoding.UTF8.GetBytes(cmb));
            foreach (var bt in hashArray) { hash.Append(bt.ToString("x")); }
            return hash.ToString();
        }

        public static bool CompareSaltHash(string saltedHash, string password, string salt)
        {
            return saltedHash.Equals(CreateSaltedHash(password, salt));
        }
    }
}
