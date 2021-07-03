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
    /// <summary>
    /// Static class providing functionality for managing persisted Folder data
    /// </summary>
    public static class FolderRepository
    {
        /// <summary>
        /// Returns a List containing all Folders in the database, 
        /// including special Folders (AllFolders, Settings)
        /// </summary>
        /// <returns></returns>
        public static List<Folder> GetAllFolders()
        {
            var result = new List<Folder>();

            result.Add(new Folder("All items", Folder.Type.AllItems));
            result.Add(new Folder("Settings", Folder.Type.Settings));

            // Populate result with a Folder object for each unique FolderName in LoginTable
            using (IDbConnection connection = new SQLiteConnection(DatabaseHelper.ConnectionString))
            {
                string query = "SELECT DISTINCT FolderName FROM LoginTable ORDER BY FolderName ASC";
                var folderNames = connection.Query<string>(query, new DynamicParameters());
                foreach(string name in folderNames)
                {
                    result.Add(new Folder(name));
                }
                return result;
            }
        }
    }
}
