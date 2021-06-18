using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace Rememberall
{
    public class Folder
    {
        /// <summary>
        /// Enum for differentiating between different types of Folders
        /// </summary>
        public enum Type
        {
            AllItems,
            Settings,
            Folder
        }

        public string Name { get; set; }
        public Type FolderType { get; set; }
        public Path IconPath { get; }

        public Folder(string name, Type type = Type.Folder)
        {
            Name = name;
            FolderType = type;
            // Set IconPath based on FolderType
            switch (FolderType)
            {
                case Type.Folder:
                    IconPath = (Path)App.Current.FindResource("ic_folder");
                    break;
                case Type.AllItems:
                    IconPath = (Path)App.Current.FindResource("ic_allFolders");
                    break;
                case Type.Settings:
                    IconPath = (Path)App.Current.FindResource("ic_settings");
                    break;
            }
        }
    }
}
