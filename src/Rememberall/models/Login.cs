using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rememberall
{
    public class Login
    {

        public long TicksCreated { get; private set; }
        public long TicksModified { get; set; }
        public string FolderName { get; set; }
        public string FaviconPath { get; set; }
        public string Title { get; set; }
        public string Website { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Note { get; set; }
        public bool IsFavorite { get; set; }

        public Login()
        {
            TicksCreated = DateTime.Now.Ticks;
            TicksModified = TicksCreated;
        }
    }
}
