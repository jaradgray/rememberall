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
        public static readonly string ConnectionString = $@"Data Source={TempDbPath};Version=3;";

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

    }
}
