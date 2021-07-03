using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Rememberall
{
    public class GreetingViewModel : BaseINPC
    {
        #region Commands
        
        private ICommand _attemptUnlockCommand;
        public ICommand AttemptUnlockCommand
        {
            get
            {
                if (_attemptUnlockCommand == null)
                {
                    _attemptUnlockCommand = new RelayCommand(
                        param => AttemptUnlock(),
                        param => !String.IsNullOrEmpty(EnteredPassword));
                }
                return _attemptUnlockCommand;
            }
        }

        #endregion // Commands


        #region Properties

        private string _prompt;
        public string Prompt
        {
            get { return _prompt; }
            set
            {
                if (value.Equals(_prompt)) return;
                _prompt = value;
                OnPropertyChanged();
            }
        }

        private string _enteredPassword;
        public string EnteredPassword
        {
            get { return _enteredPassword; }
            set
            {
                if (value.Equals(_enteredPassword)) return;
                _enteredPassword = value;
                OnPropertyChanged();
            }
        }

        private Visibility _errorVisibility_PasswordLength;
        public Visibility ErrorVisibility_PasswordLength
        {
            get { return _errorVisibility_PasswordLength; }
            set
            {
                if (value == _errorVisibility_PasswordLength) return;
                _errorVisibility_PasswordLength = value;
                OnPropertyChanged();
            }
        }

        private Visibility _errorVisibility_IncorrectPassword;
        public Visibility ErrorVisibility_IncorrectPassword
        {
            get { return _errorVisibility_IncorrectPassword; }
            set
            {
                if (value == _errorVisibility_IncorrectPassword) return;
                _errorVisibility_IncorrectPassword = value;
                OnPropertyChanged();
            }
        }

        #endregion // Properties


        #region Constructor

        public GreetingViewModel()
        {
            // Set Prompt based on if master password has been set
            Prompt = (String.IsNullOrWhiteSpace(Properties.Settings.Default.MasterPasswordHash)) ?
                "Get started by setting your Master Password." :
                "Enter your master password to unlock your Rememberall.";

            // Hide errors
            ErrorVisibility_PasswordLength = Visibility.Collapsed;
            ErrorVisibility_IncorrectPassword = Visibility.Collapsed;
        }

        #endregion // Constructor


        #region Private methods

        private void AttemptUnlock()
        {
            // Initially hide all errors
            ErrorVisibility_PasswordLength = Visibility.Collapsed;
            ErrorVisibility_IncorrectPassword = Visibility.Collapsed;

            // Get persisted settings
            string masterPasswordHash = Properties.Settings.Default.MasterPasswordHash;
            string masterPasswordSalt = Properties.Settings.Default.MasterPasswordSalt;

            // If master password hasn't been set...
            if (String.IsNullOrWhiteSpace(masterPasswordHash))
            {
                // Verify EnteredPassword is compatible with our Crypto methods (minimum length)
                if (EnteredPassword.Length < 12)
                {
                    ErrorVisibility_PasswordLength = Visibility.Visible;
                    return;
                }

                // Set EnteredPassword as master password
                MasterPasswordHelper.SetMasterPassword(EnteredPassword);

                // Encrypt bundled database file
                DatabaseHelper.Password = EnteredPassword;
                DatabaseHelper.WriteEncryptedDatabase();

                // Proceed to unlock app
                ((MainWindowViewModel)App.Current.MainWindow.DataContext).OnMasterPasswordAccepted(EnteredPassword);
                return;
            }

            // If execution reaches here, master password has already been set.
            // Check if EnteredPassword's hash matches MasterPasswordHash
            if (CryptoHelper.VerifyHash(EnteredPassword, masterPasswordSalt, masterPasswordHash))
            {
                // Set DatabaseHelper's Password
                DatabaseHelper.Password = EnteredPassword;
                // Unlock app
                ((MainWindowViewModel)App.Current.MainWindow.DataContext).OnMasterPasswordAccepted(EnteredPassword);
            }
            else
            {
                // EnteredPassword's hash doesn't match MasterPasswordHash; show error
                ErrorVisibility_IncorrectPassword = Visibility.Visible;
            }
        }

        #endregion // Private methods
    }
}
