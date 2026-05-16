using CineTrack.Data.Models;

namespace CineTrack.Tests.Fakes
{
    public class FakeUtilisateurRepositoryTests
    {
        [Fact]
        public async Task AddUserAsync_NouvelUtilisateur_EstAjouteListe()
        {
            var repo = new FakeUtilisateurRepository();
            var utilisateur = new Utilisateur
            {
                FullName = "Jackie Chan",
                Username = "JackieChan67",
                Password = "Password123$$123//",
                Email = "jackiechan67@gmail.com"
            };

            await repo.AddUserAsync(utilisateur);

            Assert.Single(repo.Utilisateurs);
            Assert.Equal("Jackie Chan", repo.Utilisateurs[0].FullName);
        }

        [Fact]
        public async Task GetByUsernameAsync_UtilisateurPresent_RetourneUtilisateur()
        {
            var repo = new FakeUtilisateurRepository();
            await repo.AddUserAsync(new Utilisateur
            {
                FullName = "Jackie Chan",
                Username = "JackieChan67",
                Password = "Password123$$123//",
                Email = "jackiechan67@gmail.com"
            });
            var test = await repo.GetByUsernameAsync("JackieChan67");

            Assert.NotNull(test);
            Assert.Equal("JackieChan67", test.Username);
        }
        [Fact]
        public async Task GetByUsernameAsync_UtilisateurAbsent_RetourneNull()
        {
            var repo = new FakeUtilisateurRepository();
            var test = await repo.GetByUsernameAsync("GNEGNGEGNE");
            Assert.Null(test);
        }
        [Fact]
        public async Task GetByEmailAsync_UtilisateurPresent_RetourneUtilisateur()
        {
            var repo = new FakeUtilisateurRepository();
            await repo.AddUserAsync(new Utilisateur
            {
                FullName = "Jackie Chan",
                Username = "JackieChan67",
                Password = "Password123$$123//",
                Email = "jackiechan67@gmail.com"
            });
            var test = await repo.GetByEmailAsync("jackiechan67@gmail.com");

            Assert.NotNull(test);
            Assert.Equal("jackiechan67@gmail.com", test.Email);
        }
        [Fact]
        public async Task GetByEmailAsync_UtilisateurAbsent_RetourneNull()
        {
            var repo = new FakeUtilisateurRepository();
            var test = await repo.GetByEmailAsync("GNEGNGEGNE@prout.com");
            Assert.Null(test);
        }
        [Fact]
        public async Task GetUtilisateurByIdAsync_UtilisateurPresent_RetourneUtilisateur()
        {
            var repo = new FakeUtilisateurRepository();
            await repo.AddUserAsync(new Utilisateur
            {
                FullName = "Jackie Chan",
                Username = "JackieChan67",
                Password = "Password123$$123//",
                Email = "jackiechan67@gmail.com"
            });
            var test = await repo.GetUtilisateurByIdAsync(1);

            Assert.NotNull(test);
            Assert.Equal(1, test.Id);
        }
        [Fact]
        public async Task GetUtilisateurByIdAsync_UtilisateurAbsent_RetourneNull()
        {
            var repo = new FakeUtilisateurRepository();
            var test = await repo.GetUtilisateurByIdAsync(6767);
            Assert.Null(test);
        }
        [Fact]
        public async Task UpdateUserAsync_UtilisateurPresent_MiseAJourFaite()
        {
            var repo = new FakeUtilisateurRepository();
            var user = new Utilisateur
            {
                FullName = "Jackie Chan",
                Username = "JackieChan67",
                Password = "Password123$$123//",
                Email = "jackiechan67@gmail.com"
            };
            await repo.AddUserAsync(user);
            user.FullName = "Michelle Obama";
            user.Username = "MichelleO";

            await repo.UpdateUserAsync(user);

            Assert.Equal("Michelle Obama", repo.Utilisateurs[0].FullName);
            Assert.Equal("MichelleO", repo.Utilisateurs[0].Username);
        }
        [Fact]
        public async Task UpdateUserAsync_UtilisateurAbsent_MiseAJourNonFaite()
        {
            var repo = new FakeUtilisateurRepository();
            await repo.AddUserAsync(new Utilisateur
            {
                FullName = "Jackie Chan",
                Username = "JackieChan67",
                Password = "Password123$$123//",
                Email = "jackiechan67@gmail.com"
            });
            var userAbsent = new Utilisateur
            {
                Id = 666,
                FullName = "Prout",
                Username = "Username",
                Password = "PasswordPassword123",
                Email = "IamAFakeUser@gmail.com"
            };

            await repo.UpdateUserAsync(userAbsent);

            Assert.Single(repo.Utilisateurs);
            Assert.Equal("Jackie Chan", repo.Utilisateurs[0].FullName);
        }

        [Fact]
        public async Task DeleteUserAsync_UtilisateurPresent_EstSupprimer()
        {
            var repo = new FakeUtilisateurRepository();
            var user = new Utilisateur
            {
                FullName = "Jackie Chan",
                Username = "JackieChan67",
                Password = "Password123$$123//",
                Email = "jackiechan67@gmail.com"
            };

            await repo.AddUserAsync(user);

            await repo.DeleteUserAsync(user);

            Assert.Empty(repo.Utilisateurs);
        }

        [Fact]
        public async Task DeleteUserAsync_UtilisateurAbsent_SuppressionNonFaite()
        {
            var repo = new FakeUtilisateurRepository();
            await repo.AddUserAsync(new Utilisateur
            {
                FullName = "Jackie Chan",
                Username = "JackieChan67",
                Password = "Password123$$123//",
                Email = "jackiechan67@gmail.com"
            });

            var userAbsent = new Utilisateur
            {
                Id = 666,
                FullName = "Prout",
                Username = "Username",
                Password = "PasswordPassword123",
                Email = "IamAFakeUser@gmail.com"
            };
            await repo.DeleteUserAsync(userAbsent);

            Assert.Single(repo.Utilisateurs);
        }

        [Fact]
        public async Task GetUtilisateursAsync_ListeAvecUtilisateurs_RetourneLaListeComplete()
        {
            var repo = new FakeUtilisateurRepository();
            await repo.AddUserAsync(new Utilisateur
            {
                FullName = "Jackie Chan",
                Username = "JackieChan67",
                Password = "Password123$$123//",
                Email = "jackiechan67@gmail.com"
            });
            await repo.AddUserAsync(new Utilisateur
            {
                FullName = "Prout",
                Username = "Username",
                Password = "PasswordPassword123",
                Email = "IamAFakeUser@gmail.com"
            });

            var test = await repo.GetUtilisateursAsync();
            Assert.Equal(2, test.Count);
        }

        [Fact]
        public async Task GetUtilisateursAsync_ListeSansUtilisateurs_RetourneLaListeVide()
        {
            var repo = new FakeUtilisateurRepository();
            var test = await repo.GetUtilisateursAsync();
            Assert.Empty(test);
        }
    }
}
