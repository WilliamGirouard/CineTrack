using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CineTrack.Data.Services.Interfaces
{
    public interface IUtilisateurService
    {
        void SignUp(string username, string fullName, string email, string password);
        void SignIn(string username, string password);

        //Fonction de logout a ajouter
        bool IsUsernameUsed(string username);
        bool IsEmailUsed(string email);
    }
}
