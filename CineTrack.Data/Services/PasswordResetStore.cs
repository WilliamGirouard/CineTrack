using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CineTrack.Data.Services
{
    public class PasswordResetStore
    {
        public Dictionary<string, (string Code, DateTime Expiry)> Codes { get; } = new();
    }
}
