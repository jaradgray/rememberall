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

        public static void SaveLogin(Login login)
        {
            if (LoginExists(login)) UpdateLogin(login);
            else InsertLogin(login);
        }

        /// <summary>
        /// Returns true if a record exists in the database's LoginTable whose TicksCreated
        /// column matches the given Login's TicksCreated property, otherwise returns false
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        public static bool LoginExists(Login login)
        {
            using (IDbConnection connection = new SQLiteConnection(CONNECTION_STRING))
            {
                string query = "SELECT * FROM LoginTable WHERE TicksCreated = " + login.TicksCreated;
                var output = connection.Query<Login>(query, new DynamicParameters());
                return output.Count() > 0;
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

        /// <summary>
        /// Inserts into the database's LoginTable a new record representing the given Login
        /// </summary>
        /// <param name="login"></param>
        private static void InsertLogin(Login login)
        {
            using (IDbConnection connection = new SQLiteConnection(CONNECTION_STRING))
            {
                string columnList = "(TicksCreated, TicksModified, FolderName, FaviconPath, Title, Website, Email, Username, Password, Note, IsFavorite)";
                string valuesList = "(@TicksCreated, @TicksModified, @FolderName, @FaviconPath, @Title, @Website, @Email, @Username, @Password, @Note, @IsFavorite)";
                string sql = "INSERT INTO LoginTable " + columnList + " VALUES " + valuesList;
                connection.Execute(sql, login);
            }
        }

        /// <summary>
        /// Updates the record representing the given Login in the database's LoginTable with the given Login's new property values
        /// </summary>
        /// <param name="login"></param>
        private static void UpdateLogin(Login login)
        {
            using (IDbConnection connection = new SQLiteConnection(CONNECTION_STRING))
            {
                string sql = "UPDATE LoginTable SET "
                    + "TicksModified = @TicksModified"
                    + ",FolderName = @FolderName"
                    + ",FaviconPath = @FaviconPath"
                    + ",Title = @Title"
                    + ",Website = @Website"
                    + ",Email = @Email"
                    + ",Username = @Username"
                    + ",Password = @Password"
                    + ",Note = @Note"
                    + ",IsFavorite = @IsFavorite"
                    + " WHERE TicksCreated = @TicksCreated";
                connection.Execute(sql, login);
            }
        }

        #endregion // Private methods

    }
}
