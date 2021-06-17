using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Rememberall
{
    public class MainWindowViewModel : BaseINPC
    {
        #region Commands

        private ICommand _addLoginCommand;
        public ICommand AddLoginCommand
        {
            get
            {
                if (_addLoginCommand == null)
                {
                    _addLoginCommand = new RelayCommand(
                        param => ShowAddLoginView() /* execute */,
                        param => SelectedFolder.FolderType != Folder.Type.Settings /* canExecute */);
                }
                return _addLoginCommand;
            }
        }

        private ICommand _editLoginCommand;
        public ICommand EditLoginCommand
        {
            get
            {
                if (_editLoginCommand == null)
                {
                    _editLoginCommand = new RelayCommand(
                        param => ShowEditLoginView(),
                        param => true);
                }
                return _editLoginCommand;
            }
        }

        #endregion // Commands

        #region Properties and backing fields

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

                // Update LoginsMatchingFilter
                // TODO will also need to consider search text for filtering eventually
                LoginsMatchingFilter = LoginRepository.GetLoginsInFolder(_selectedFolder);

                // Update CurrentDetailsView
                CurrentDetailsView = (SelectedFolder.FolderType == Folder.Type.Settings) ? m_settingsVM : null;
            }
        }

        // TODO should this be here or in a separate UserControl's ViewModel?
        private List<Login> _loginsMatchingFilter;
        public List<Login> LoginsMatchingFilter
        {
            get { return _loginsMatchingFilter; }
            set
            {
                if (value == _loginsMatchingFilter) return;
                _loginsMatchingFilter = value;
                OnPropertyChanged();
            }
        }

        private Login _selectedLogin;
        public Login SelectedLogin
        {
            get { return _selectedLogin; }
            set
            {
                if (value == _selectedLogin) return;
                _selectedLogin = value;
                OnPropertyChanged();

                // Update CurrentDetailsView
                CurrentDetailsView = (SelectedLogin == null) ? null : m_loginDetailsVM;

                // Update m_loginDetailsVM's Login property
                m_loginDetailsVM.Login = SelectedLogin;
            }
        }

        private BaseINPC _currentDetailsView;
        public BaseINPC CurrentDetailsView
        {
            get { return _currentDetailsView; }
            set
            {
                if (value == _currentDetailsView) return;
                _currentDetailsView = value;
                OnPropertyChanged();
            }
        }

        #endregion // Properties and backing fields

        #region Private members

        private LoginDetailsViewModel m_loginDetailsVM = new LoginDetailsViewModel();
        private SettingsViewModel m_settingsVM = new SettingsViewModel();

        #endregion // Private members

        public MainWindowViewModel()
        {
            AllFolders = FolderRepository.GetAllFolders();
            SelectedFolder = AllFolders[0];
        }


        #region Private methods

        private void ShowAddLoginView()
        {
            Console.WriteLine("TODO set CurrentDetailsView to an instance of AddEditLoginViewModel to add a new Login");
        }

        private void ShowEditLoginView()
        {
            Console.WriteLine("TODO set CurrentDetailsView to an instance of AddEditLoginViewModel to edit current Login");
        }

        #endregion // Private methods
    }
}
