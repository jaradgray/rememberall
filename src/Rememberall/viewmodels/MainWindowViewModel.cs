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
                        param => AddLoginCanExecute() /* canExecute */);
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
                    DisplayedLogins = null;
                    CurrentDetailsView = null;
                    return;
                }

                // Update DisplayedLogins
                UpdateDisplayedLogins();

                // Update CurrentDetailsView
                CurrentDetailsView = (SelectedFolder.FolderType == Folder.Type.Settings) ? m_settingsVM : null;
            }
        }

        // TODO should this be here or in a separate UserControl's ViewModel?
        private ObservableCollection<Login> _displayedLogins;
        public ObservableCollection<Login> DisplayedLogins
        {
            get { return _displayedLogins; }
            set
            {
                if (value == _displayedLogins) return;
                _displayedLogins = value;
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

        private string _filterText;
        public string FilterText
        {
            get { return _filterText; }
            set
            {
                if (value.Equals(_filterText)) return;
                _filterText = value;
                OnPropertyChanged();

                // Update DisplayedLogins
                UpdateDisplayedLogins();
            }
        }

        private int _selectedSortModeIndex = 2; // TODO initialize from Settings
        public int SelectedSortModeIndex
        {
            get { return _selectedSortModeIndex; }
            set
            {
                if (value == _selectedSortModeIndex) return;
                _selectedSortModeIndex = value;
                OnPropertyChanged();

                // Update DisplayedLogins
                // TODO ideally we just sort, but maybe we need to refresh via UpdateDisplayedLogins() ?
                DisplayedLogins = new ObservableCollection<Login>(SortLogins(DisplayedLogins));

                // TODO save new value to Settings
            }
        }

        private GreetingViewModel _greetingVM;
        public GreetingViewModel GreetingVM
        {
            get { return _greetingVM; }
            set
            {
                if (value == _greetingVM) return;
                _greetingVM = value;
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
            GreetingVM = new GreetingViewModel(); // show the greeting/login screen
        }

        #endregion // Constructor


        #region Public methods

        public void OnMasterPasswordAccepted(string enteredPassword)
        {
            // TODO Set master password in memory

            // Create temporary database we'll connect to during this app session
            DatabaseHelper.CreateTempDatabase(enteredPassword);

            // Initialize view
            GreetingVM = null;
            AllFolders = FolderRepository.GetAllFolders();
            SelectedFolder = AllFolders[0];
        }

        #endregion // Public methods


        #region Private methods

        private void ShowAddLoginView()
        {
            m_addEditLoginVM = new AddEditLoginViewModel(SelectedFolder);
            SelectedLogin = null;
            CurrentDetailsView = m_addEditLoginVM;
        }

        private bool AddLoginCanExecute()
        {
            // Disable AddLogin button if:
            //  - user is currently editing a Login
            //  - or, user is currently looking at settings
            if ((CurrentDetailsView != null && CurrentDetailsView == m_addEditLoginVM)
                || (CurrentDetailsView != null && CurrentDetailsView == m_settingsVM))
            {
                return false;
            }
            return true;
        }

        private void ShowEditLoginView()
        {
            m_addEditLoginVM = new AddEditLoginViewModel(SelectedLogin);
            CurrentDetailsView = m_addEditLoginVM;
        }

        /// <summary>
        /// Saves the given Login to the database and refreshes the UI to show the saved Login as selected
        /// </summary>
        /// <param name="login"></param>
        private void SaveLogin(Login login)
        {
            // Do some final processing/validation of the Login before giving it to the database
            // TODO should this final processing be done somewhere else?
            if (string.IsNullOrEmpty(login.FolderName)) login.FolderName = Login.BLANK_FOLDER_NAME;
            // TicksModified and (possibly) TicksCreated
            long now = DateTime.Now.Ticks;
            if (!LoginRepository.LoginExists(login)) login.TicksCreated = now;
            login.TicksModified = now;
            // Set FaviconPath
            login.FaviconPath = (String.IsNullOrEmpty(login.Website)) ?
                @"pack://application:,,,/res/images/ic_favicon_default.png" :
                @"https://www.google.com/s2/favicons?sz=128&domain_url=" + login.Website;

            LoginRepository.SaveLogin(login);
            // Refresh AllFolders from the database
            bool isAllFoldersSelected = SelectedFolder.FolderType == Folder.Type.AllItems; // persist whether SelectedFolder is the AllItems Folder
            AllFolders = FolderRepository.GetAllFolders();
            // Set SelectedFolder to AllItems or the given Login's Folder
            SelectedFolder = (isAllFoldersSelected) ? AllFolders[0] : AllFolders.FirstOrDefault(folder => folder.Name.Equals(login.FolderName));
            // (Try to) Set SelectedLogin to the Login we just saved
            SelectedLogin = DisplayedLogins.FirstOrDefault(loginInList => loginInList.TicksCreated == login.TicksCreated);
        }

        /// <summary>
        /// Deletes SelectedLogin from the database and refreshes UI to reflect the changes
        /// </summary>
        private void DeleteSelectedLogin()
        {
            // Prompt user for confirmation
            var prompt = new YesNoDialog()
            {
                Message = $"Delete all data for {(string.IsNullOrEmpty(SelectedLogin.TitleOrWebsite) ? "login" : SelectedLogin.TitleOrWebsite)}?",
                AffirmativeText = "Delete",
                NegativeText = "Cancel"
            };
            prompt.Owner = App.Current.MainWindow;
            if (prompt.ShowDialog() != true) return;

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

        /// <summary>
        /// Sets DisplayedLogins to the subset of Logins in the database that are in the SelectedFolder and satisfy
        /// the Login filtering criteria, ordered based on SelectedSortModeIndex
        /// </summary>
        private void UpdateDisplayedLogins()
        {
            // Get all Logins in the selected Folder
            var loginsInSelectedFolder = LoginRepository.GetLoginsInFolder(SelectedFolder);
            // Filter Logins in list based on FilterText
            var filteredLogins = (String.IsNullOrEmpty(FilterText)) ? loginsInSelectedFolder : loginsInSelectedFolder.Where(login => SatisfiesLoginFilter(login));
            // Sort Logins based on SelectedSortModeIndex
            var sortedFilteredLogins = SortLogins(filteredLogins);
            // Set DisplayedLogins to the sorted, filtered list
            DisplayedLogins = new ObservableCollection<Login>(sortedFilteredLogins);
        }

        /// <summary>
        /// Returns true if the given Login satisfies the filtering criteria; false otherwise
        /// </summary>
        /// <param name="login"></param>
        /// <returns>true if the given Login satisfies the filtering criteria; false otherwise</returns>
        private bool SatisfiesLoginFilter(Login login)
        {
            // Passes filter criteria if:
            //  FilterText is in Title, Website, Email, Username, Password, or Note
            
            bool isFilterTextInTitle = !String.IsNullOrEmpty(login.Title) && login.Title.ToLower().Contains(FilterText.ToLower());
            bool isFilterTextInWebsite = !String.IsNullOrEmpty(login.Website) && login.Website.ToLower().Contains(FilterText.ToLower());
            bool isFilterTextInEmail = !String.IsNullOrEmpty(login.Email) && login.Email.ToLower().Contains(FilterText.ToLower());
            bool isFilterTextInUsername = !String.IsNullOrEmpty(login.Username) && login.Username.ToLower().Contains(FilterText.ToLower());
            bool isFilterTextInPassword= !String.IsNullOrEmpty(login.Password) && login.Password.ToLower().Contains(FilterText.ToLower());
            bool isFilterTextInNote = !String.IsNullOrEmpty(login.Note) && login.Note.ToLower().Contains(FilterText.ToLower());

            if (isFilterTextInTitle
                || isFilterTextInWebsite
                || isFilterTextInEmail
                || isFilterTextInUsername
                || isFilterTextInPassword
                || isFilterTextInNote) return true;

            return false;
        }

        /// <summary>
        /// Returns a new Collection containing the elements in the given collection, ordered based on SelectedSortModeIndex
        /// </summary>
        /// <param name="logins"></param>
        /// <returns></returns>
        private IOrderedEnumerable<Login> SortLogins(IEnumerable<Login> logins)
        {
            // Note: make sure the order of cases matches the order the sort modes appear in the view
            switch (SelectedSortModeIndex)
            {
                case 0:
                    return logins.OrderBy(login => login.TitleOrWebsite);
                case 1:
                    return logins.OrderBy(login => login.TicksCreated);
                case 2:
                    return logins.OrderByDescending(login => login.TicksCreated);
                case 3:
                    return logins.OrderBy(login => login.TicksModified);
                case 4:
                    return logins.OrderByDescending(login => login.TicksModified);
                default:
                    return null;
            }
        }

        #endregion // Private methods
    }
}
