using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace Rememberall
{
    public static class LoginRepository
    {
        private const string CONNECTION_STRING = @"Data Source=.\data\dummy.db;Version=3;"; // the connection string for the database we'll connect to

        /// <summary>
        /// Returns a List containing all Login objects in the database's LoginTable
        /// whose FolderName property matches the given Folder's Name property, or
        /// all Logins if the given Folder.FolderType property is AllFolders
        /// </summary>
        /// <param name="folder"></param>
        /// <returns></returns>
        public static List<Login> GetLoginsInFolder(Folder folder)
        {
            if (folder.FolderType == Folder.Type.AllItems) return GetAllLogins();

            using (IDbConnection connection = new SQLiteConnection(CONNECTION_STRING))
            {
                string query = "SELECT * FROM LoginTable WHERE FolderName = '" + folder.Name + "'";
                var output = connection.Query<Login>(query, new DynamicParameters());
                return new List<Login>(output);
            }
        }

        /// <summary>
        /// Returns a List of Login objects representing each record in the database's LoginTable
        /// </summary>
        /// <returns></returns>
        private static List<Login> GetAllLogins()
        {
            using (IDbConnection connection = new SQLiteConnection(CONNECTION_STRING))
            {
                string query = "SELECT * FROM LoginTable";
                var output = connection.Query<Login>(query, new DynamicParameters());
                return new List<Login>(output);
            }
        }
    }
}
