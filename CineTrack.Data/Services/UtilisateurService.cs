using CineTrack.Data.Models;
using CineTrack.Data.Repositories.Interfaces;
using CineTrack.Data.Services.Interfaces;


namespace CineTrack.Data.Services
{
    public class UtilisateurService : IUtilisateurService
    {
        private readonly IUtilisateurRepository _utilisateurRepository;

        public UtilisateurService(IUtilisateurRepository utilisateurRepository)
        {
            _utilisateurRepository = utilisateurRepository;
        }

        public bool IsEmailUsed(string email)
        {
            return _utilisateurRepository.GetByEmail(email) != null;
        }

        public bool IsUsernameUsed(string username)
        {
            return _utilisateurRepository.GetByUsername(username) != null;
        }

        public void SignIn(string username, string password)
        {
            Utilisateur userVerif = _utilisateurRepository.GetByUsername(username);
            if (userVerif == null)
            {
                throw new Exception("Invalid credentials");            
            }

            if (!HashService.CompareHashToPassword(password, userVerif.Password)) 
            {
                throw new Exception("Invalid credentials");
            }
            //Faire systeme de token
        }

        public void SignUp(string username, string fullName, string email, string password)
        {
            if (IsEmailUsed(email))
            {
                throw new Exception("Email already used");
            }
            if (IsUsernameUsed(username))
            {
                throw new Exception("Username already used");
            }
            string hashedPassword = HashService.PasswordHasher(password);
            Utilisateur user = new Utilisateur
            {
                Username = username,
                FullName = fullName,
                Password = hashedPassword,
                Email = email
            };
            _utilisateurRepository.AddUser(user);
        }
        //Fonction de logout
    }
}
