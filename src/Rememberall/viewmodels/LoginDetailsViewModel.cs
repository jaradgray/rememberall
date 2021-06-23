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

        private ICommand _openUrlCommand;
        public ICommand OpenUrlCommand
        {
            get
            {
                if (_openUrlCommand == null)
                {
                    _openUrlCommand = new RelayCommand(
                        param => OpenUrl((string)param),
                        param => !String.IsNullOrEmpty((string)param));
                }
                return _openUrlCommand;
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

        /// <summary>
        /// Formats the given string into a url that can be opened by a web broswer, and opens
        /// it in the default browser.
        /// </summary>
        /// <param name="url"></param>
        private void OpenUrl(string url)
        {
            url = url.Trim();
            if (!url.StartsWith("http://") && !url.StartsWith("https://")) url = "http://" + url;
            System.Diagnostics.Process.Start(url);
        }

        private void TogglePasswordVisibility()
        {
            Console.WriteLine("TODO toggle password visibility");
        }

        #endregion // Private methods
    }
}
