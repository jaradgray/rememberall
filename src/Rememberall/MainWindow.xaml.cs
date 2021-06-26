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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Button m_MaximizeButton, m_RestoreButton;
        private MainWindowViewModel m_ViewModel;

        public MainWindow()
        {
            InitializeComponent();

            // TODO remove the ViewModel stuff here and use databinding
            // Get a ViewModel
            m_ViewModel = new MainWindowViewModel();

            // Handle changes to ViewModel's data
            m_ViewModel.PropertyChanged += (sender, args) =>
            {
                switch (args.PropertyName)
                {
                    case nameof(MainWindowViewModel.AllFolders):
                        AllFolders_Change(m_ViewModel.AllFolders);
                        break;
                }
            };

            // Initialize UI to ViewModel's data
            AllFolders_Change(m_ViewModel.AllFolders);

            DataContext = m_ViewModel;
        }

        private void AllFolders_Change(List<Folder> allFolders)
        {
            ListView_Folders.ItemsSource = allFolders;
        }

        /// <summary>
        /// Override OnApplyTemplate() so we have access to Controls declared in the ControlTemplate.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            // Get references to Buttons declared in ControlTemplate so we can add click event handlers to them
            var MinimizeButton = (Button)this.Template.FindName("MinimizeButton", this);
            m_MaximizeButton = (Button)this.Template.FindName("MaximizeButton", this);
            m_RestoreButton = (Button)this.Template.FindName("RestoreButton", this);
            var CloseButton = (Button)this.Template.FindName("CloseButton", this);

            // Add click event handlers to Buttons declared in the ControlTemplate
            MinimizeButton.Click += MinimizeButton_Click;
            m_MaximizeButton.Click += MaximizeRestoreButton_Click;
            m_RestoreButton.Click += MaximizeRestoreButton_Click;
            CloseButton.Click += CloseButton_Click;

            // Sync displayed maximize/restore Button to WindowState
            this.RefreshMaximizeRestoreButton();
        }


        #region Event handlers

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void MaximizeRestoreButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = (this.WindowState == WindowState.Maximized) ? WindowState.Normal : WindowState.Maximized;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Refreshes maximize/restore Buttons when WindowState changes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_StateChanged(object sender, EventArgs e)
        {
            this.RefreshMaximizeRestoreButton();
        }

        #endregion // Event handlers


        #region Private helpers

        /// <summary>
        /// Sets displayed maximize/restore Button based on WindowState.
        /// </summary>
        private void RefreshMaximizeRestoreButton()
        {
            if (this.WindowState == WindowState.Maximized)
            {
                m_MaximizeButton.Visibility = Visibility.Collapsed;
                m_RestoreButton.Visibility = Visibility.Visible;
            }
            else
            {
                m_MaximizeButton.Visibility = Visibility.Visible;
                m_RestoreButton.Visibility = Visibility.Collapsed;
            }
        }

        #endregion // Private helpers
    }
}
