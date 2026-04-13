using CineTrack.Data.Context;
using CineTrack.Data.Models;
using CineTrack.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace CineTrack.Data.Repositories
{
    public class UtilisateurRepository : IUtilisateurRepository
    {
        private readonly CineTrackDbContext _context;
        public UtilisateurRepository(CineTrackDbContext context) 
        {
            _context = context;
        }

        public async Task<List<Utilisateur>> GetUtilisateursAsync()
        {
            return await _context.Utilisateurs.ToListAsync();
        }
        public async Task<Utilisateur?> GetUtilisateurByIdAsync(int id)
        {
            return await _context.Utilisateurs.FindAsync(id);
        }
        public async Task<Utilisateur?> GetByUsernameAsync(string username) {
            return await _context.Utilisateurs.FirstOrDefaultAsync(u => u.Username == username);
        }
        public async Task<Utilisateur?> GetByEmailAsync(string email) {
            return await _context.Utilisateurs.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task AddUserAsync(Utilisateur user) {
            _context.Utilisateurs.Add(user);
            await _context.SaveChangesAsync();
            Console.WriteLine(@"User added: " + user.Username);
        }

        public async Task UpdateUserAsync(Utilisateur user)
        {
            _context.Utilisateurs.Update(user);
            await _context.SaveChangesAsync();
            Console.WriteLine(@"User Updated: " + user.Username);
        }
        public async Task DeleteUserAsync(Utilisateur user)
        {
            if (await GetByUsernameAsync(user.Username) != null)
            {
                Console.WriteLine(@"User Deleted: " + user.Username);
                _context.Utilisateurs.Remove(user);
                await _context.SaveChangesAsync();
            }
        }
    }
}
