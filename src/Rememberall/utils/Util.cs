using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Rememberall
{
    public static class Util
    {
        public const string USER_DATA_DIR_NAME = "data";
        public const string DB_FILENAME = "data.db";
        public const string DB_BACKUP_FILENAME = "dummy_BACKUP.db";

        /// <summary>
        /// Asynchronously makes a copy of the database file in the data directory.
        /// </summary>
        public static void BackupDatabaseFile()
        {
            Task.Run(() =>
            {
                string src = Path.Combine(Environment.CurrentDirectory, USER_DATA_DIR_NAME, DB_FILENAME);
                string dst = Path.Combine(Environment.CurrentDirectory, USER_DATA_DIR_NAME, DB_BACKUP_FILENAME);
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
