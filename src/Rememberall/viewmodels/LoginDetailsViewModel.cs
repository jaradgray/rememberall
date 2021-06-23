using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Rememberall
{
    public class LoginDetailsViewModel : BaseINPC
    {
        #region Commands

        private ICommand _openWebsiteCommand;
        public ICommand OpenWebsiteCommand
        {
            get
            {
                if (_openWebsiteCommand == null)
                {
                    _openWebsiteCommand = new RelayCommand(
                        param => OpenWebsite((string)param),
                        param => !String.IsNullOrEmpty((string)param));
                }
                return _openWebsiteCommand;
            }
        }

        private ICommand _copyCommand;
        public ICommand CopyCommand
        {
            get
            {
                if (_copyCommand == null)
                {
                    _copyCommand = new RelayCommand(
                        param => System.Windows.Clipboard.SetText((string)param),
                        param => !String.IsNullOrEmpty((string)param));
                }
                return _copyCommand;
            }
        }

        private ICommand _togglePasswordVisibilityCommand;
        public ICommand TogglePasswordVisibilityCommand
        {
            get
            {
                if (_togglePasswordVisibilityCommand == null)
                {
                    _togglePasswordVisibilityCommand = new RelayCommand(
                        param => TogglePasswordVisibility(),
                        param => true);
                }
                return _togglePasswordVisibilityCommand;
            }
        }

        #endregion // Commands


        #region Properties

        private Login _login;
        public Login Login
        {
            get { return _login; }
            set
            {
                if (value == _login) return;
                _login = value;
                OnPropertyChanged();
            }
        }

        #endregion // Properties


        #region Private methods

        private void OpenWebsite(string url)
        {
            Console.WriteLine("TODO open website");
        }

        private void TogglePasswordVisibility()
        {
            Console.WriteLine("TODO toggle password visibility");
        }

        #endregion // Private methods
    }
}
