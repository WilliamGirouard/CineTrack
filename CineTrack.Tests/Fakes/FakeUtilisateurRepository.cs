using CineTrack.Data.Models;
using CineTrack.Data.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CineTrack.Tests.Fakes
{
    public class FakeUtilisateurRepository : IUtilisateurRepository
    {
        public List<Utilisateur> Utilisateurs { get; } = new List<Utilisateur>();

        public Task AddUserAsync(Utilisateur utilisateur)
        {
            utilisateur.Id = Utilisateurs.Count + 1;
            Utilisateurs.Add(utilisateur);
            return Task.CompletedTask;
        }
        public Task DeleteUserAsync(Utilisateur user)
        {
            Utilisateurs.Remove(user);
            return Task.CompletedTask;
        }

        public Task<Utilisateur?> GetByEmailAsync(string email)
            => Task.FromResult(Utilisateurs.FirstOrDefault(u => u.Email == email));


        public Task<Utilisateur?> GetByUsernameAsync(string username)
        => Task.FromResult(Utilisateurs.FirstOrDefault(u => u.Username == username));


        public Task<Utilisateur?> GetUtilisateurByIdAsync(int id)
            => Task.FromResult(Utilisateurs.FirstOrDefault(u => u.Id == id));

        public Task<List<Utilisateur>> GetUtilisateursAsync() 
            => Task.FromResult(Utilisateurs);

        public Task UpdateUserAsync(Utilisateur user)
        {
            var existing = Utilisateurs.FirstOrDefault(u => u.Id == user.Id);
            if (existing != null) 
            {
                existing.FullName = user.FullName;
                existing.Username = user.Username;
                existing.Password = user.Password;
                existing.Email = user.Email;
            }
            return Task.CompletedTask;
        }
    }
}
