using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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

        #endregion // Commands


        #region Properties

        public Visibility TitleVisibility
        {
            get
            {
                return String.IsNullOrEmpty(_login.Title) ? Visibility.Collapsed : Visibility.Visible;
            }
        }
        public Visibility WebsiteVisibility
        {
            get
            {
                return String.IsNullOrEmpty(_login.Website) ? Visibility.Collapsed : Visibility.Visible;
            }
        }
        public Visibility UsernameVisibility
        {
            get
            {
                return String.IsNullOrEmpty(_login.Username) ? Visibility.Collapsed : Visibility.Visible;
            }
        }
        public Visibility EmailVisibility
        {
            get
            {
                return String.IsNullOrEmpty(_login.Email) ? Visibility.Collapsed : Visibility.Visible;
            }
        }
        public Visibility PasswordVisibility
        {
            get
            {
                return String.IsNullOrEmpty(_login.Password) ? Visibility.Collapsed : Visibility.Visible;
            }
        }
        public Visibility NoteVisibility
        {
            get
            {
                return String.IsNullOrEmpty(_login.Note) ? Visibility.Collapsed : Visibility.Visible;
            }
        }

        private Login _login;
        public Login Login
        {
            get { return _login; }
            set
            {
                if (value == _login) return;
                _login = value;
                OnPropertyChanged();

                // Refresh properties dependent on Login
                if (_login == null) return;
                OnPropertyChanged(nameof(TitleVisibility));
                OnPropertyChanged(nameof(WebsiteVisibility));
                OnPropertyChanged(nameof(UsernameVisibility));
                OnPropertyChanged(nameof(EmailVisibility));
                OnPropertyChanged(nameof(PasswordVisibility));
                OnPropertyChanged(nameof(NoteVisibility));
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

        #endregion // Private methods
    }
}
