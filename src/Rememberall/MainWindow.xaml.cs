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
        private List<Folder> m_Folders;

        public MainWindow()
        {
            InitializeComponent();

            // TODO do this in the ViewModel
            m_Folders = FolderRepository.GetAllFolders();

            ListView_Folders.ItemsSource = m_Folders;
        }
    }
}
