using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rememberall
{
    public static class MasterPasswordHelper
    {
        public static void SetMasterPassword(string password)
        {
            // Generate a new salt value
            byte[] salt = CryptoHelper.NewSalt(CryptoHelper.SaltBitSize / 8);
            string saltString = Convert.ToBase64String(salt);
            // Get hash value of given password + generated salt
            string hash = CryptoHelper.GetHash(password, saltString);
            // persist values in Settings
            Properties.Settings.Default.MasterPasswordHash = hash;
            Properties.Settings.Default.MasterPasswordSalt = saltString;
            Properties.Settings.Default.Save();
        }
    }
}
