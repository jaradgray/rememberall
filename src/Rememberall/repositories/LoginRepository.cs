using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        #region Public methods

        /// <summary>
        /// Returns a List containing all Login objects in the database's LoginTable
        /// whose FolderName property matches the given Folder's Name property, or
        /// all Logins if the given Folder.FolderType property is AllFolders
        /// </summary>
        /// <param name="folder"></param>
        /// <returns></returns>
        public static ObservableCollection<Login> GetLoginsInFolder(Folder folder)
        {
            if (folder == null) return null;
            if (folder.FolderType == Folder.Type.AllItems) return GetAllLogins();

            using (IDbConnection connection = new SQLiteConnection(CONNECTION_STRING))
            {
                string query = "SELECT * FROM LoginTable WHERE FolderName = '" + folder.Name + "'";
                var output = connection.Query<Login>(query, new DynamicParameters());
                return new ObservableCollection<Login>(output);
            }
        }

        /// <summary>
        /// Removes from the database's LoginTable the record whose TicksCreated column
        /// matches the given Login's TicksCreated property
        /// </summary>
        /// <param name="login"></param>
        public static void DeleteLogin(Login login)
        {
            using (IDbConnection connection = new SQLiteConnection(CONNECTION_STRING))
            {
                string sql = "DELETE FROM LoginTable WHERE TicksCreated = @TicksCreated";
                connection.Execute(sql, login);
            }
        }

        #endregion // Public methods


        #region Private methods

        /// <summary>
        /// Returns a List of Login objects representing each record in the database's LoginTable
        /// </summary>
        /// <returns></returns>
        private static ObservableCollection<Login> GetAllLogins()
        {
            using (IDbConnection connection = new SQLiteConnection(CONNECTION_STRING))
            {
                string query = "SELECT * FROM LoginTable";
                var output = connection.Query<Login>(query, new DynamicParameters());
                return new ObservableCollection<Login>(output);
            }
        }

        #endregion // Private methods

    }
}
