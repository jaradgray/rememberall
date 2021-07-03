using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Rememberall
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Exit(object sender, ExitEventArgs e)
        {
            // Wipe the temporary database file if it exists and delete it
            if (File.Exists(DatabaseHelper.TempDbPath))
            {
                using (FileStream fs = File.Open(DatabaseHelper.TempDbPath, FileMode.Open))
                {
                    // Set the length of the FileStream to 0 and flush it to the physical file
                    fs.SetLength(0);
                    fs.Flush();
                }
                File.Delete(DatabaseHelper.TempDbPath);
            }
        }

    }
}
