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
        private MainWindowViewModel m_ViewModel;

        public MainWindow()
        {
            InitializeComponent();

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
        }

        private void AllFolders_Change(List<Folder> allFolders)
        {
            ListView_Folders.ItemsSource = allFolders;
        }
    }
}
