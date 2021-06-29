using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Rememberall
{
    /// <summary>
    /// Interaction logic for GreetingUserControl.xaml
    /// </summary>
    public partial class GreetingUserControl : UserControl
    {
        public GreetingUserControl()
        {
            InitializeComponent();

            RefreshShowHidePassword();
        }


        #region UI event handlers

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!PasswordBox.Password.Equals(TextBox.Text))
            {
                PasswordBox.Password = TextBox.Text;
            }
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (!TextBox.Text.Equals(PasswordBox.Password))
            {
                TextBox.Text = PasswordBox.Password;
            }
        }

        private void Button_HidePassword_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.IsPasswordObscured = true;
            Properties.Settings.Default.Save();
            RefreshShowHidePassword();
        }

        private void Button_ShowPassword_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.IsPasswordObscured = false;
            Properties.Settings.Default.Save();
            RefreshShowHidePassword();
        }

        #endregion // UI event handlers


        #region Private methods

        /// <summary>
        /// Sets visibilities of master password Controls based on persisted IsPasswordObscured setting,
        /// then sets the visible input Control's caret position to the end of its content and focuses
        /// the visible input Control.
        /// </summary>
        private void RefreshShowHidePassword()
        {
            if (Properties.Settings.Default.IsPasswordObscured)
            {
                TextBox.Visibility = Visibility.Collapsed;
                Button_HidePassword.Visibility = TextBox.Visibility;

                PasswordBox.Visibility = Visibility.Visible;
                Button_ShowPassword.Visibility = PasswordBox.Visibility;
                SetPasswordBoxCaretPosition(PasswordBox.Password.Length);
                PasswordBox.Focus();
            }
            else
            {
                TextBox.Visibility = Visibility.Visible;
                Button_HidePassword.Visibility = TextBox.Visibility;
                TextBox.CaretIndex = TextBox.Text.Length;
                TextBox.Focus();

                PasswordBox.Visibility = Visibility.Collapsed;
                Button_ShowPassword.Visibility = PasswordBox.Visibility;
            }
        }

        /// <summary>
        /// Sets the PasswordBox's selection to start at the given index and a length of 0.
        /// 
        /// Found this on StackOverflow: https://stackoverflow.com/a/1046920
        /// </summary>
        /// <param name="index"></param>
        private void SetPasswordBoxCaretPosition(int index)
        {
            PasswordBox.GetType().GetMethod("Select", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
                .Invoke(PasswordBox, new object[] { index, 0 /* selection length */ });
        }

        #endregion // Private methods

    }
}
