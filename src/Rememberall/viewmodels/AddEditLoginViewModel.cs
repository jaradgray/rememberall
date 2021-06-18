using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rememberall
{
    public class AddEditLoginViewModel : BaseINPC
    {
        // Properties and backing fields

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

        private string _heading;
        public string Heading { get { return _heading; }
            set
            {
                if (value.Equals(_heading)) return;
                _heading = value;
                OnPropertyChanged();
            }
        }


        // Constructors

        /// <summary>
        /// Creates a new AddEditLoginViewModel configured for adding a new Login.
        /// </summary>
        public AddEditLoginViewModel()
        {
            Heading = "Add Login";
            Login = new Login();
        }

        /// <summary>
        /// Creates a new AddEditLoginViewModel configured for editing the given Login.
        /// </summary>
        /// <param name="login"></param>
        public AddEditLoginViewModel(Login login)
        {
            Heading = "Edit Login";
            Login = new Login(login); // create a new Login based on given Login, so we can update its data but only persist it when user clicks Save
        }
    }
}
