using CineTrack.Data.Models;

namespace CineTrack.Tests.Modeles
{
    public class UtilisateurTests
    {
        [Fact]
        public void Constructeur_SansParametres_InitialiseDateCreationAujourdhui()
        {
            var utilisateur = new Utilisateur();

            Assert.Equal(DateTime.Today, utilisateur.DateCreation.Date);
        }

        [Fact]
        public void Constructeur_SansParametres_DateCreationNonNull()
        {
            var utilisateur = new Utilisateur();

            Assert.NotEqual(default(DateTime), utilisateur.DateCreation);
        }

        [Fact]
        public void Proprietes_AssignationDirecte_ValeursConservees()
        {
            var utilisateur = new Utilisateur
            {
                FullName = "Jackie Chan",
                Username = "JackieChan67",
                Password = "Password123$$123$$",
                Email = "jackiechan67@gmail.com"
            };

            Assert.Equal("Jackie Chan", utilisateur.FullName);
            Assert.Equal("JackieChan67", utilisateur.Username);
            Assert.Equal("Password123$$123$$", utilisateur.Password);
            Assert.Equal("jackiechan67@gmail.com", utilisateur.Email);
            //Cense se mettre par defaut, donc je verifie si ça marche
            Assert.Equal(EnumRole.user, utilisateur.Role);
        }

        [Theory]
        [InlineData("Carol Denver", "CarolD", "Password123$$123$$", "carolDenver@gmail.com")]
        [InlineData("Jackie Chan", "JackieChan67", "Password123$$123$$", "jackiechan67@gmail.com")]
        [InlineData("Jean Peuplus", "JPP", "Password123$$123$$", "jpp@gmail.com")]
        public void Proprietes_DiversNomsEtPasswordEtEmail_BienAssignes(string fullname, string  username, string password, string email)
        {
            var utilisateur = new Utilisateur
            {
                FullName = fullname,
                Username = username,
                Password = password,
                Email = email
            };
            Assert.Equal(fullname, utilisateur.FullName);
            Assert.Equal(username, utilisateur.Username);
            Assert.Equal(password, utilisateur.Password);
            Assert.Equal(email, utilisateur.Email);
        }
    }
}
