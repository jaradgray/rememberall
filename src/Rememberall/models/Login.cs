using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rememberall
{
    public class Login
    {
        // TODO image for website favicon?
        public string Title { get; set; }
        public string Website { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Note { get; set; }
        public Folder Folder { get; set; }
        public long TicksCreated { get; private set; }
        public long TicksModified { get; set; }
        public bool IsFavorite { get; set; }

        public Login()
        {
            TicksCreated = DateTime.Now.Ticks;
            TicksModified = TicksCreated;
        }
    }
}
