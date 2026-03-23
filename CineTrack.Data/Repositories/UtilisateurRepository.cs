using CineTrack.Data.Context;
using CineTrack.Data.Models;
using CineTrack.Data.Repositories.Interfaces;


namespace CineTrack.Data.Repositories
{
    public class UtilisateurRepository : IUtilisateurRepository
    {
        private readonly CineTrackDbContext _context;
        public UtilisateurRepository(CineTrackDbContext context) 
        {
            _context = context;
        }

        public List<Utilisateur> GetUtilisateurs()
        {
            return _context.Utilisateurs.ToList();
        }
        public Utilisateur GetUtilisateurById(int id)
        {
            return _context.Utilisateurs.Find(id);
        }
        public Utilisateur GetByUsername(string username) {
            return _context.Utilisateurs.FirstOrDefault(u => u.Username == username);
        }
        public Utilisateur GetByEmail(string email) {
            return _context.Utilisateurs.FirstOrDefault(u => u.Email == email);
        }

        public void AddUser(Utilisateur user) {
            _context.Utilisateurs.Add(user);
            _context.SaveChanges();
            Console.WriteLine(@"User added: " + user.Username);
        }

        public void UpdateUser(Utilisateur user)
        {
            _context.Utilisateurs.Update(user);
            _context.SaveChanges();
            Console.WriteLine(@"User Updated: " + user.Username);
        }
        public void DeleteUser(Utilisateur user)
        {
            if (GetByUsername(user.Username) != null)
            {
                Console.WriteLine(@"User Deleted: " + user.Username);
                _context.Utilisateurs.Remove(user);
                _context.SaveChanges();
            }
        }
    }
}
