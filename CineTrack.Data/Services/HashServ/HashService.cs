using BCrypt.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CineTrack.Data.Services.HashServ
{
    public static class HashService
    {
        private const int SaltRounds = 12;
        public static string PasswordHasher(string password)
        { 
            return BCrypt.Net.BCrypt.HashPassword(password, SaltRounds);
        }

        public static bool CompareHashToPassword(string password, string hash)
        {
            return BCrypt.Net.BCrypt.Verify(password, hash);
        }
    }
}
