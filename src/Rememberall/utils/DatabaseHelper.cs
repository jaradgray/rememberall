using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Rememberall
{
    /// <summary>
    /// Helper class for managing database files used by the app.
    /// </summary>
    public static class DatabaseHelper
    {
        public static readonly string EncryptedDbPath = @".\data\dummy.db"; // path to db that is bundled with the app and overwritten with encrypted data
        public static readonly string BackupEncryptedDbPath = @".\data\data_BACKUP.db"; // path to the copy of the encrypted database file
        public static readonly string TempDbPath = @".\data\~temp"; // path to unencrypted temporary db that the app actually connects to
        public static readonly string ConnectionString = $@"Data Source={TempDbPath};Version=3;"; // connection string to the database the app connects to

        /// <summary>
        /// The password this class uses to encrypt and decrypt database files.
        /// Must be set before invoking any method that uses it.
        /// </summary>
        public static string Password { private get; set; }

        /// <summary>
        /// Reads the unencrypted database file's data, encrypts it with the Password
        /// member, and overwrites the bundled database file with the encrypted data.
        /// </summary>
        /// <param name="password"></param>
        public static void WriteEncryptedDatabase()
        {
            if (String.IsNullOrEmpty(Password))
                throw new InvalidOperationException("Member 'Password' is not set");
            
            // Get unencrypted db's bytes.
            //  Read temp db file if it exists, otherwise read bundled db file
            string path = (File.Exists(TempDbPath)) ? TempDbPath : EncryptedDbPath;
            byte[] unencrypted = File.ReadAllBytes(path);
            // Encrypt them
            byte[] encrypted = CryptoHelper.SimpleEncryptWithPassword(unencrypted, Password);
            // Write them to bundled db file
            File.WriteAllBytes(EncryptedDbPath, encrypted);
        }

        /// <summary>
        /// Reads the encrypted database file's data, decrypts it with the Password
        /// member, and writes the decrypted data to a file on-disk.
        /// </summary>
        public static void CreateTempDatabase()
        {
            if (String.IsNullOrEmpty(Password))
                throw new InvalidOperationException("Member 'Password' is not set");

            // Get encrypted db's bytes
            byte[] encrypted = File.ReadAllBytes(EncryptedDbPath);
            // Decrypt them
            byte[] decrypted = CryptoHelper.SimpleDecryptWithPassword(encrypted, Password);
            // Write them to temporary db file
            File.WriteAllBytes(TempDbPath, decrypted);
        }

        /// <summary>
        /// Asynchronously makes a copy of the encrypted database file in the data directory.
        /// </summary>
        public static void BackupEncryptedDatabase()
        {
            Task.Run(() =>
            {
                string src = EncryptedDbPath;
                string dst = BackupEncryptedDbPath;
                try
                {
                    File.Copy(src, dst, true);
                }
                catch (Exception e)
                {
                    MessageBox.Show($"Unable to backup database file:\n{src}\n\nMaybe it's missing or renamed?\n\n{e.ToString()}", "Database Backup Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    Console.Error.WriteLine(e.ToString());
                }
            });
        }

    }
}
