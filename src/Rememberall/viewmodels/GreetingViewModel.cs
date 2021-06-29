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
                        param => !String.IsNullOrEmpty(Password));
                }
                return _attemptUnlockCommand;
            }
        }

        #endregion // Commands


        #region Properties

        private string _password;
        public string Password
        {
            get { return _password; }
            set
            {
                if (value.Equals(_password)) return;
                _password = value;
                OnPropertyChanged();
            }
        }

        #endregion // Properties


        #region Private methods

        private void AttemptUnlock()
        {
            Console.WriteLine($"Attempt unlock with password: \"{Password}\"");
        }

        #endregion // Private methods
    }
}
