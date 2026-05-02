using CineTrack.Data.Context;
using CineTrack.Data.Models;
using CineTrack.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace CineTrack.Data.Repositories
{
    public class UtilisateurRepository : IUtilisateurRepository
    {
        private readonly IDbContextFactory<CineTrackDbContext> _context;
        public UtilisateurRepository(IDbContextFactory<CineTrackDbContext> context) 
        {
            _context = context;
        }

        public async Task<List<Utilisateur>> GetUtilisateursAsync()
        {
            using var context = _context.CreateDbContext();
            return await context.Utilisateurs.ToListAsync();
        }
        public async Task<Utilisateur?> GetUtilisateurByIdAsync(int id)
        {
            using var context = _context.CreateDbContext();
            return await context.Utilisateurs.FindAsync(id);
        }
        public async Task<Utilisateur?> GetByUsernameAsync(string username) 
        {
            using var context = _context.CreateDbContext();
            return await context.Utilisateurs.FirstOrDefaultAsync(u => u.Username == username);
        }
        public async Task<Utilisateur?> GetByEmailAsync(string email) 
        {
            using var context = _context.CreateDbContext();
            return await context.Utilisateurs.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task AddUserAsync(Utilisateur user) 
        {
            using var context = _context.CreateDbContext();
            context.Utilisateurs.Add(user);
            await context.SaveChangesAsync();
            Console.WriteLine(@"User added: " + user.Username);
        }

        public async Task UpdateUserAsync(Utilisateur user)
        {
            using var context = _context.CreateDbContext();
            context.Utilisateurs.Update(user);
            await context.SaveChangesAsync();
            Console.WriteLine(@"User Updated: " + user.Username);
        }
        public async Task DeleteUserAsync(Utilisateur user)
        {
            using var context = _context.CreateDbContext();
            if (await GetByUsernameAsync(user.Username) != null)
            {
                Console.WriteLine(@"User Deleted: " + user.Username);
                context.Utilisateurs.Remove(user);
                await context.SaveChangesAsync();
            }
        }
    }
}
