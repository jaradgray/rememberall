using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Shapes;

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

        private ICommand _toggleObscurePasswordCommand;
        public ICommand ToggleObscurePasswordCommand
        {
            get
            {
                if (_toggleObscurePasswordCommand == null)
                {
                    _toggleObscurePasswordCommand = new RelayCommand(
                        param => IsPasswordObscured = !IsPasswordObscured,
                        param => true);
                }
                return _toggleObscurePasswordCommand;
            }
        }

        #endregion // Commands


        #region Properties

        public Visibility TitleVisibility
        {
            get
            {
                if (Login == null) return Visibility.Visible;
                return String.IsNullOrEmpty(_login.Title) ? Visibility.Collapsed : Visibility.Visible;
            }
        }
        public Visibility WebsiteVisibility
        {
            get
            {
                if (Login == null) return Visibility.Visible;
                return String.IsNullOrEmpty(_login.Website) ? Visibility.Collapsed : Visibility.Visible;
            }
        }
        public Visibility UsernameVisibility
        {
            get
            {
                if (Login == null) return Visibility.Visible;
                return String.IsNullOrEmpty(_login.Username) ? Visibility.Collapsed : Visibility.Visible;
            }
        }
        public Visibility EmailVisibility
        {
            get
            {
                if (Login == null) return Visibility.Visible;
                return String.IsNullOrEmpty(_login.Email) ? Visibility.Collapsed : Visibility.Visible;
            }
        }
        public Visibility PasswordVisibility
        {
            get
            {
                if (Login == null) return Visibility.Visible;
                return String.IsNullOrEmpty(_login.Password) ? Visibility.Collapsed : Visibility.Visible;
            }
        }
        public Visibility NoteVisibility
        {
            get
            {
                if (Login == null) return Visibility.Visible;
                return String.IsNullOrEmpty(_login.Note) ? Visibility.Collapsed : Visibility.Visible;
            }
        }

        /// <summary>
        /// Gets Login.Password or corresponding bullet points depending on IsPasswordObscured.
        /// </summary>
        public string DisplayedPasswordText
        {
            get
            {
                if (Login == null || String.IsNullOrEmpty(Login.Password)) return "";
                if (IsPasswordObscured)
                {
                    string obscuredPassword = "";
                    foreach (char c in Login.Password)
                    {
                        obscuredPassword += "\u2022";
                    }
                    return obscuredPassword;
                }
                return Login.Password;
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

                // Refresh readonly properties dependent on Login
                OnPropertyChanged(nameof(TitleVisibility));
                OnPropertyChanged(nameof(WebsiteVisibility));
                OnPropertyChanged(nameof(UsernameVisibility));
                OnPropertyChanged(nameof(EmailVisibility));
                OnPropertyChanged(nameof(PasswordVisibility));
                OnPropertyChanged(nameof(NoteVisibility));

                OnPropertyChanged(nameof(DisplayedPasswordText));
            }
        }

        /// <summary>
        /// Gets the resource of the icon for the "toggle password visibility" button based on IsPasswordObscured
        /// </summary>
        public Path IconPath_TogglePasswordButton
        {
            get
            {
                string key = IsPasswordObscured ? "ic_show_password" : "ic_hide_password";
                return (Path)App.Current.FindResource(key);
            }
        }

        private bool _isPasswordObscured;
        private bool IsPasswordObscured
        {
            get { return _isPasswordObscured; }
            set
            {
                _isPasswordObscured = value;
                // Note: no need to call OnPropertyChagned() because nothing is bound directly to this property

                // Refresh readonly properties dependent on IsPasswordObscured
                OnPropertyChanged(nameof(IconPath_TogglePasswordButton));
                OnPropertyChanged(nameof(DisplayedPasswordText));

                // Persist value in Settings
                Properties.Settings.Default.IsPasswordObscured = _isPasswordObscured;
                Properties.Settings.Default.Save();
            }
        }

        #endregion // Properties


        #region Constructor

        public LoginDetailsViewModel()
        {
            IsPasswordObscured = Properties.Settings.Default.IsPasswordObscured;
        }

        #endregion // Constructor


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
