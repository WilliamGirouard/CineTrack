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
        Task<List<Utilisateur>> GetUtilisateursAsync();
        Task<Utilisateur?> GetUtilisateurByIdAsync(int id);
        Task<Utilisateur?> GetByUsernameAsync(string username);
        Task<Utilisateur?> GetByEmailAsync(string email);
        Task AddUserAsync(Utilisateur user);
        Task UpdateUserAsync(Utilisateur user);
        Task DeleteUserAsync(Utilisateur user);
    }
}
