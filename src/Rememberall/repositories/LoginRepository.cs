using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rememberall
{
    public static class LoginRepository
    {
        private const string CONNECTION_STRING = @"Data Source=.\data\dummy.db;Version=3;"; // the connection string for the database we'll connect to

        /// <summary>
        /// Returns a List containing all Login objects in the database whose
        /// Folder property matches the given Folder, or all Logins if the
        /// given Folder.FolderType property is AllFolders
        /// </summary>
        /// <param name="folder"></param>
        /// <returns></returns>
        public static List<Login> GetLoginsInFolder(Folder folder)
        {
            var result = new List<Login>();
            switch (folder.FolderType)
            {
                case Folder.Type.Folder:
                    // TODO get Logins from database
                    result.Add(new Login()
                    {
                        Title = "Login 1 in folder",
                        Username = "the username"
                    });
                    result.Add(new Login()
                    {
                        Title = "Login 2 in folder",
                        Username = "Mandrake"
                    });
                    result.Add(new Login()
                    {
                        Title = "Login 3 in folder",
                        Username = "Leviathan"
                    });
                    break;
                case Folder.Type.AllItems:
                    // TODO get all Logins from database
                    result.Add(new Login()
                    {
                        Title = "Login 1 in folder",
                        Username = "the username"
                    });
                    result.Add(new Login()
                    {
                        Title = "Login 2 in folder",
                        Username = "Mandrake"
                    });
                    result.Add(new Login()
                    {
                        Title = "Login 3 in folder",
                        Username = "Leviathan"
                    });
                    result.Add(new Login()
                    {
                        Title = "Login 4 in different folder",
                        Username = "Hanzel"
                    });
                    result.Add(new Login()
                    {
                        Title = "Login 5 in different folder",
                        Username = "Gretel"
                    });
                    break;
            }
            return result;
        }
    }
}
