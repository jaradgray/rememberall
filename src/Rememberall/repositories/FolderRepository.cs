using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            result.Add(new Folder("Hello", Folder.Type.AllItems));
            result.Add(new Folder("There", Folder.Type.Settings));

            // TODO populate result with actual Folders from the database
            result.Add(new Folder("Folder 1", Folder.Type.Folder));
            result.Add(new Folder("Folder 2", Folder.Type.Folder));
            result.Add(new Folder("Folder 3", Folder.Type.Folder));

            return result;
        }
    }
}
