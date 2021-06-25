using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Rememberall
{
    public class SettingsViewModel : BaseINPC
    {
        #region Commands

        private ICommand _openFolderCommand;
        public ICommand OpenFolderCommand
        {
            get
            {
                if (_openFolderCommand == null)
                {
                    _openFolderCommand = new RelayCommand(
                        param => Process.Start(DatabaseFolderPath),
                        param => !String.IsNullOrEmpty(DatabaseFolderPath));
                }
                return _openFolderCommand;
            }
        }

        #endregion // Commands


        #region Properties

        private string _databaseFolderPath;
        public string DatabaseFolderPath
        {
            get { return _databaseFolderPath; }
            set
            {
                if (value.Equals(_databaseFolderPath)) return;
                _databaseFolderPath = value;
                OnPropertyChanged();
            }
        }

        #endregion // Properties


        #region Constructor

        public SettingsViewModel()
        {
            // Set DatabaseFolderPath to the path of the bundled database file's directory
            string DATA_FOLDER_NAME = "data";
            DatabaseFolderPath = Path.Combine(Environment.CurrentDirectory, DATA_FOLDER_NAME);
        }

        #endregion // Constructor
    }
}
