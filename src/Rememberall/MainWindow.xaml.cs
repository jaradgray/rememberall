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

            // TODO do this properly
            m_Folders = new List<Folder>();
            m_Folders.Add(new Folder("Hello", Folder.Type.AllItems));
            m_Folders.Add(new Folder("There", Folder.Type.Settings));
            m_Folders.Add(new Folder("Folder 1", Folder.Type.Folder));
            m_Folders.Add(new Folder("Folder 2", Folder.Type.Folder));
            m_Folders.Add(new Folder("Folder 3", Folder.Type.Folder));

            ListView_Folders.ItemsSource = m_Folders;
        }
    }
}
