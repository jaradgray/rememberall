using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Rememberall
{
    public class SettingsViewModel : BaseINPC
    {
        #region Commands

        private ICommand _openFolderCommand;
        public ICommand OpenFolderCommand
        {
            get
            {
                if (_openFolderCommand == null)
                {
                    _openFolderCommand = new RelayCommand(
                        param => Process.Start(DatabaseFolderPath),
                        param => !String.IsNullOrEmpty(DatabaseFolderPath));
                }
                return _openFolderCommand;
            }
        }

        private ICommand _attemptChangePasswordCommand;
        public ICommand AttemptChangePasswordCommand
        {
            get
            {
                if (_attemptChangePasswordCommand == null)
                {
                    _attemptChangePasswordCommand = new RelayCommand(
                        param => AttemptChangePassword(),
                        param => !String.IsNullOrEmpty(EnteredMasterPassword) && !String.IsNullOrEmpty(EnteredNewMasterPassword) && !String.IsNullOrEmpty(EnteredConfirmedNewMasterPassword));
                }
                return _attemptChangePasswordCommand;
            }
        }

        #endregion // Commands


        #region Properties

        private string _databaseFolderPath;
        public string DatabaseFolderPath
        {
            get { return _databaseFolderPath; }
            set
            {
                if (value.Equals(_databaseFolderPath)) return;
                _databaseFolderPath = value;
                OnPropertyChanged();
            }
        }

        private string _enteredMasterPassword;
        public string EnteredMasterPassword
        {
            get { return _enteredMasterPassword; }
            set
            {
                if (value.Equals(_enteredMasterPassword)) return;
                _enteredMasterPassword = value;
                OnPropertyChanged();
            }
        }

        private string _enteredNewMasterPassword;
        public string EnteredNewMasterPassword
        {
            get { return _enteredNewMasterPassword; }
            set
            {
                if (value.Equals(_enteredNewMasterPassword)) return;
                _enteredNewMasterPassword = value;
                OnPropertyChanged();
            }
        }

        private string _enteredConfirmedNewMasterPassword;
        public string EnteredConfirmedNewMasterPassword
        {
            get { return _enteredConfirmedNewMasterPassword; }
            set
            {
                if (value.Equals(_enteredConfirmedNewMasterPassword)) return;
                _enteredConfirmedNewMasterPassword = value;
                OnPropertyChanged();
            }
        }

        private Visibility _errorVisibility_IncorrectMasterPassword;
        public Visibility ErrorVisibility_IncorrectMasterPassword
        {
            get { return _errorVisibility_IncorrectMasterPassword; }
            set
            {
                if (value == _errorVisibility_IncorrectMasterPassword) return;
                _errorVisibility_IncorrectMasterPassword = value;
                OnPropertyChanged();
            }
        }

        private Visibility _errorVisibility_MasterPasswordLength;
        public Visibility ErrorVisibility_MasterPasswordLength
        {
            get { return _errorVisibility_MasterPasswordLength; }
            set
            {
                if (value == _errorVisibility_MasterPasswordLength) return;
                _errorVisibility_MasterPasswordLength = value;
                OnPropertyChanged();
            }
        }

        private Visibility _errorVisibility_MasterPasswordMatch;
        public Visibility ErrorVisibility_MasterPasswordMatch
        {
            get { return _errorVisibility_MasterPasswordMatch; }
            set
            {
                if (value == _errorVisibility_MasterPasswordMatch) return;
                _errorVisibility_MasterPasswordMatch = value;
                OnPropertyChanged();
            }
        }

        #endregion // Properties


        #region Constructor

        public SettingsViewModel()
        {
            // Set DatabaseFolderPath to the path of the bundled database file's directory
            string DATA_FOLDER_NAME = "data";
            DatabaseFolderPath = Path.Combine(Environment.CurrentDirectory, DATA_FOLDER_NAME);

            // Hide errors
            ShowHideChangeMasterPasswordErrors(false, false, false);
        }

        #endregion // Constructor


        #region Private methods

        private void AttemptChangePassword()
        {
            // Verify EnteredMasterPassword's hash matches persisted master password's hash
            string toMatch = Properties.Settings.Default.MasterPasswordHash;
            string salt = Properties.Settings.Default.MasterPasswordSalt;
            if (!CryptoHelper.VerifyHash(EnteredMasterPassword, salt, toMatch))
            {
                // Incorrect master password; show appropriate error and return
                ShowHideChangeMasterPasswordErrors(true, false, false);
                return;
            }

            // Verify new passwords match
            if (!EnteredNewMasterPassword.Equals(EnteredConfirmedNewMasterPassword))
            {
                // New passwords don't match; show appropriate error and return
                ShowHideChangeMasterPasswordErrors(false, false, true);
                return;
            }

            // Verify new password's length
            if (EnteredNewMasterPassword.Length < 12)
            {
                // New password too short; show appropriate error and return
                ShowHideChangeMasterPasswordErrors(false, true, false);
                return;
            }

            // Update view
            ShowHideChangeMasterPasswordErrors(false, false, false);
            EnteredMasterPassword = "";
            EnteredNewMasterPassword = "";
            EnteredConfirmedNewMasterPassword = "";
            // Change master password
            MasterPasswordHelper.SetMasterPassword(EnteredNewMasterPassword);

            // TODO re-encrypt database with new master password
            // TODO tell MainWindowVM master password has changed
        }

        /// <summary>
        /// Convenience method for setting all ErrorVisibility_* properties based on given arguments.
        /// ErrorVisibility_* properties are set to either Visible or Collapsed.
        /// </summary>
        /// <param name="isIncorrectErrorVisible"></param>
        /// <param name="isLengthErrorVisible"></param>
        /// <param name="isMatchErrorVisible"></param>
        private void ShowHideChangeMasterPasswordErrors(bool isIncorrectErrorVisible, bool isLengthErrorVisible, bool isMatchErrorVisible)
        {
            ErrorVisibility_IncorrectMasterPassword = (isIncorrectErrorVisible) ? Visibility.Visible : Visibility.Collapsed;
            ErrorVisibility_MasterPasswordLength = (isLengthErrorVisible) ? Visibility.Visible : Visibility.Collapsed;
            ErrorVisibility_MasterPasswordMatch = (isMatchErrorVisible) ? Visibility.Visible : Visibility.Collapsed;
        }

        #endregion // Private methods
    }
}
