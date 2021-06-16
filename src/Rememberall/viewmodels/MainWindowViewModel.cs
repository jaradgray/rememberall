using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rememberall
{
    public class MainWindowViewModel : BaseINPC
    {
        private List<Folder> _allFolders;
        public List<Folder> AllFolders
        {
            get { return _allFolders; }
            set
            {
                if (value == _allFolders) return;
                _allFolders = value;
                OnPropertyChanged();
            }
        }

        private Folder _selectedFolder;
        public Folder SelectedFolder
        {
            get { return _selectedFolder; }
            set
            {
                if (value == _selectedFolder) return;
                _selectedFolder = value;
                OnPropertyChanged();
            }
        }

        public MainWindowViewModel()
        {
            AllFolders = FolderRepository.GetAllFolders();
            SelectedFolder = AllFolders[0];
        }
    }
}
