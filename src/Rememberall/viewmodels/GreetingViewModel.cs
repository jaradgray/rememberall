using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        #endregion // Properties


        #region Constructor

        public GreetingViewModel()
        {
            // Set Prompt based on if master password has been set
            Prompt = (String.IsNullOrEmpty(Properties.Settings.Default.MasterPasswordHash)) ?
                "Get started by setting your Master Password." :
                "Enter your master password to unlock your Rememberall.";
        }

        #endregion // Constructor


        #region Private methods

        private void AttemptUnlock()
        {
            string masterPasswordHash = Properties.Settings.Default.MasterPasswordHash;
            string masterPasswordSalt = Properties.Settings.Default.MasterPasswordSalt;

            // If master password hasn't been set...
            if (String.IsNullOrEmpty(masterPasswordHash))
            {
                // TODO verify EnteredPassword is compatible with our Crypto methods (minimum length)

                // Set EnteredPassword as master password
                MasterPasswordHelper.SetMasterPassword(EnteredPassword);

                // TODO overwrite bundled db file with its encrypted bytes

                // Proceed to unlock app
                ((MainWindowViewModel)App.Current.MainWindow.DataContext).OnMasterPasswordAccepted(EnteredPassword);
                return;
            }

            // If execution reaches here, master password has already been set.
            // Check if EnteredPassword's hash matches MasterPasswordHash
            if (CryptoHelper.VerifyHash(EnteredPassword, masterPasswordSalt, masterPasswordHash))
            {
                // Unlock app
                ((MainWindowViewModel)App.Current.MainWindow.DataContext).OnMasterPasswordAccepted(EnteredPassword);
            }
            else
            {
                // EnteredPassword's hash doesn't match MasterPasswordHash
                // Update Prompt
                Prompt = "Incorrect Master Password.";
            }
        }

        #endregion // Private methods
    }
}
