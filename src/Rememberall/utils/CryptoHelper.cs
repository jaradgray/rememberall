using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Rememberall
{
    /// <summary>
    /// Helper class for all things cryptographic.
    /// </summary>
    public static class CryptoHelper
    {
        public static readonly int SaltBitSize = 64;


        /// <summary>
        /// Hashes the given string + salt string and returns the Base64 encoded
        /// string representing the hash value.
        /// </summary>
        /// <param name="toHash"></param>
        /// <param name="saltString"></param>
        /// <returns></returns>
        public static string GetHash(string toHash, string saltString)
        {
            // Convert strings to byte arrays
            byte[] plaintext = Encoding.UTF8.GetBytes(toHash);
            byte[] salt = Encoding.UTF8.GetBytes(saltString);

            // Generate hash value
            byte[] hash = GenerateSaltedHash(plaintext, salt);
            return Convert.ToBase64String(hash);
        }

        /// <summary>
        /// Returns whether the hash value of the given plaintext and salt strings
        /// matches the given hash string.
        /// </summary>
        /// <param name="plaintext"></param>
        /// <param name="salt"></param>
        /// <param name="hash"></param>
        /// <returns></returns>
        public static bool VerifyHash(string plaintext, string salt, string hash)
        {
            return hash.Equals(GetHash(plaintext, salt));
        }

        /// <summary>
        /// From: https://stackoverflow.com/a/2138588
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public static byte[] NewSalt(int size)
        {
            // Generate a cryptographic random number
            RNGCryptoServiceProvider randy = new RNGCryptoServiceProvider();
            byte[] buff = new byte[size];
            randy.GetBytes(buff);

            return buff;

            // Return a Base64 string representation of the random number
            //return Convert.ToBase64String(buff);
        }

        /// <summary>
        /// Based on https://stackoverflow.com/a/2138588
        /// </summary>
        /// <param name="plaintext"></param>
        /// <param name="salt"></param>
        /// <returns></returns>
        private static byte[] GenerateSaltedHash(byte[] plaintext, byte[] salt)
        {
            // Combine plaintext and salt arrays into a new byte array
            byte[] toHash = new byte[plaintext.Length + salt.Length];
            for (int i = 0; i < plaintext.Length; i++)
                toHash[i] = plaintext[i];
            for (int i = 0; i < salt.Length; i++)
                toHash[plaintext.Length + i] = salt[i];

            using (var algorithm = SHA256.Create())
            {
                return algorithm.ComputeHash(toHash);
            }
        }

    }
}
