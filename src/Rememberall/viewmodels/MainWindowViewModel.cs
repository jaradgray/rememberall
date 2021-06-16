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

        public MainWindowViewModel()
        {
            AllFolders = FolderRepository.GetAllFolders();
        }
    }
}
