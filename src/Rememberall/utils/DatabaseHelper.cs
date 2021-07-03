using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rememberall
{
    /// <summary>
    /// Helper class for managing database files used by the app.
    /// </summary>
    public static class DatabaseHelper
    {
        public static readonly string EncryptedDbPath = @".\data\dummy.db"; // path to db that is bundled with the app and overwritten with encrypted data
        public static readonly string TempDbPath = @".\data\~temp"; // path to unencrypted temporary db that the app actually connects to
        public static readonly string ConnectionString = $@"Data Source={TempDbPath};Version=3;"; // connection string to the database the app connects to

        /// <summary>
        /// Reads the unencrypted database file's data, encrypts it with the given
        /// password, and overwrites the bundled database file with the encrypted data.
        /// </summary>
        /// <param name="password"></param>
        public static void WriteEncryptedDatabase(string password)
        {
            // Get unencrypted db's bytes.
            //  Read temp db file if it exists, otherwise read bundled db file
            string path = (File.Exists(TempDbPath)) ? TempDbPath : EncryptedDbPath;
            byte[] unencrypted = File.ReadAllBytes(path);
            // Encrypt them
            byte[] encrypted = CryptoHelper.SimpleEncryptWithPassword(unencrypted, password);
            // Write them to bundled db file
            File.WriteAllBytes(EncryptedDbPath, encrypted);
        }

        /// <summary>
        /// Reads the encrypted database file's data, decrypts it with the given
        /// password, and writes the decrypted data to a file on-disk.
        /// </summary>
        /// <param name="password"></param>
        public static void CreateTempDatabase(string password)
        {
            // Get encrypted db's bytes
            byte[] encrypted = File.ReadAllBytes(EncryptedDbPath);
            // Decrypt them
            byte[] decrypted = CryptoHelper.SimpleDecryptWithPassword(encrypted, password);
            // Write them to temporary db file
            File.WriteAllBytes(TempDbPath, decrypted);
        }

    }
}
