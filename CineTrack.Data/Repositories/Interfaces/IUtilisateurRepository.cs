using CineTrack.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CineTrack.Data.Repositories.Interfaces
{
    public interface IUtilisateurRepository
    {
        List<Utilisateur> GetUtilisateurs();
        Utilisateur GetUtilisateurById(int id);
        Utilisateur GetByUsername(string username);
        Utilisateur GetByEmail(string email);
        void AddUser(Utilisateur user);
        void UpdateUser(Utilisateur user);
        void DeleteUser(Utilisateur user);
    }
}
