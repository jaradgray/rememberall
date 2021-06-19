using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
                        param => SelectedFolder == null || SelectedFolder.FolderType != Folder.Type.Settings /* canExecute */);
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

        private ICommand _saveLoginCommand;
        public ICommand SaveLoginCommand
        {
            get
            {
                if (_saveLoginCommand == null)
                {
                    _saveLoginCommand = new RelayCommand(
                        param => SaveLogin((Login)param),
                        param => true);
                }
                return _saveLoginCommand;
            }
        }

        private ICommand _deleteSelectedLoginCommand;
        public ICommand DeleteSelectedLoginCommand
        {
            get
            {
                if (_deleteSelectedLoginCommand == null)
                {
                    _deleteSelectedLoginCommand = new RelayCommand(
                        param => DeleteSelectedLogin(),
                        param => SelectedLogin != null);
                }
                return _deleteSelectedLoginCommand;
            }
        }

        private ICommand _cancelAddEditLoginCommand;
        public ICommand CancelAddEditLoginCommand
        {
            get
            {
                if (_cancelAddEditLoginCommand == null)
                {
                    _cancelAddEditLoginCommand = new RelayCommand(
                    param => CancelAddEditLogin(),
                    param => true);
                }
                return _cancelAddEditLoginCommand;
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

                if (_selectedFolder == null)
                {
                    LoginsMatchingFilter = null;
                    CurrentDetailsView = null;
                    return;
                }

                // Update LoginsMatchingFilter
                // TODO will also need to consider search text for filtering eventually
                LoginsMatchingFilter = LoginRepository.GetLoginsInFolder(_selectedFolder);

                // Update CurrentDetailsView
                CurrentDetailsView = (SelectedFolder.FolderType == Folder.Type.Settings) ? m_settingsVM : null;
            }
        }

        // TODO should this be here or in a separate UserControl's ViewModel?
        private ObservableCollection<Login> _loginsMatchingFilter;
        public ObservableCollection<Login> LoginsMatchingFilter
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
        private AddEditLoginViewModel m_addEditLoginVM;

        #endregion // Private members


        #region Constructor

        public MainWindowViewModel()
        {
            AllFolders = FolderRepository.GetAllFolders();
            SelectedFolder = AllFolders[0];
        }

        #endregion // Constructor


        #region Private methods

        private void ShowAddLoginView()
        {
            m_addEditLoginVM = new AddEditLoginViewModel();
            SelectedLogin = null;
            CurrentDetailsView = m_addEditLoginVM;
        }

        private void ShowEditLoginView()
        {
            m_addEditLoginVM = new AddEditLoginViewModel(SelectedLogin);
            CurrentDetailsView = m_addEditLoginVM;
        }

        /// <summary>
        /// Saves the given Login to the database and refreshes the UI to reflect the changes
        /// </summary>
        /// <param name="login"></param>
        private void SaveLogin(Login login)
        {
            LoginRepository.SaveLogin(login);
            AllFolders = FolderRepository.GetAllFolders();
            SelectedFolder = AllFolders.FirstOrDefault(folder => folder.Name.Equals(login.FolderName));
            SelectedLogin = LoginsMatchingFilter.First(loginInList => loginInList.TicksCreated == login.TicksCreated);
        }

        /// <summary>
        /// Deletes SelectedLogin from the database and refreshes UI to reflect the changes
        /// </summary>
        private void DeleteSelectedLogin()
        {
            // Delete SelectedLogin from the database
            LoginRepository.DeleteLogin(SelectedLogin);

            // Refresh AllFolders from the database
            string selectedFolderName = SelectedFolder.Name; // persist SelectedFolder's Name to use below
            AllFolders = FolderRepository.GetAllFolders();

            // Set SelectedFolder
            //  - to deleted Login's Folder if it's in the list
            //  - to the "All items" Folder if not
            Folder folderToSelect = AllFolders.FirstOrDefault(folder => folder.Name.Equals(selectedFolderName));
            if (folderToSelect == null) folderToSelect = AllFolders[0];
            SelectedFolder = folderToSelect;
        }

        private void CancelAddEditLogin()
        {
            // Update CurrentDetailsView
            CurrentDetailsView = (SelectedLogin == null) ? null : m_loginDetailsVM;
        }

        #endregion // Private methods
    }
}
