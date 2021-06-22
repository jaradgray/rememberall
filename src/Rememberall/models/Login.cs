using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rememberall
{
    public class Login
    {
        public const string BLANK_FOLDER_NAME = "Uncategorized";

        public long TicksCreated { get; set; }
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

        public string TitleOrWebsite
        {
            get
            {
                return string.IsNullOrEmpty(Title) ? Website : Title;
            }
        }

        public string UsernameOrEmail
        {
            get
            {
                return String.IsNullOrEmpty(Username) ? Email : Username;
            }
        }

        public Login()
        {
            FolderName = BLANK_FOLDER_NAME;
        }

        /// <summary>
        /// Creates a new Login whose properties match the given Login's properties.
        /// </summary>
        /// <param name="toCopy"></param>
        public Login(Login toCopy)
        {
            TicksCreated = toCopy.TicksCreated;
            TicksModified = toCopy.TicksModified;
            FolderName = toCopy.FolderName;
            FaviconPath = toCopy.FaviconPath;
            Title = toCopy.Title;
            Website = toCopy.Website;
            Email = toCopy.Email;
            Username = toCopy.Username;
            Password = toCopy.Password;
            Note = toCopy.Note;
            IsFavorite = toCopy.IsFavorite;
        }
    }
}
