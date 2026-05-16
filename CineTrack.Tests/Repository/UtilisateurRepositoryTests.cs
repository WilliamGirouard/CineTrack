using CineTrack.Data.Context;
using CineTrack.Data.Models;
using CineTrack.Data.Repositories;
using CineTrack.Tests.Context;
using CineTrack.Tests.Fakes;
using Microsoft.EntityFrameworkCore;


namespace CineTrack.Tests.Repository
{
    public class UtilisateurRepositoryTests
    {
      
        [Fact]
        public async Task AddUserAsync_NouvelUtilisateur_EstPersiste()
        {
            var factory = DbContextFactoryTest.Creer();
            var repo = new UtilisateurRepository(factory);

            await repo.AddUserAsync(new Utilisateur
            {
                FullName = "Jackie Chan",
                Username = "JackieChan67",
                Password = "Password123$$123//",
                Email = "jackiechan67@gmail.com"
            });
            using var context = factory.CreateDbContext();
            Assert.Equal(1, await context.Utilisateurs.CountAsync());
        }

        [Fact]
        public async Task GetByUsernameAsync_UtilisateurPresent_RetourneUtilisateur()
        {
            var factory = DbContextFactoryTest.Creer();
            var repo = new UtilisateurRepository(factory);

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
            var factory = DbContextFactoryTest.Creer();
            var repo = new UtilisateurRepository(factory);

            var test = await repo.GetByUsernameAsync("TESUNFANFARRONTOI");
            Assert.Null(test);
        }

        [Fact]
        public async Task GetByEmailAsync_UtilisateurPresent_RetourneUtilisateur()
        {
            var factory = DbContextFactoryTest.Creer();
            var repo = new UtilisateurRepository(factory);

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
            var factory = DbContextFactoryTest.Creer();
            var repo = new UtilisateurRepository(factory);

            var test = await repo.GetByEmailAsync("TESUNFANFARRONTOI@gmail.com");
            Assert.Null(test);
        }

        [Fact]
        public async Task GetByUtilisateurIdAsync_UtilisateurPresent_RetourneUtilisateur()
        {
            var factory = DbContextFactoryTest.Creer();
            var repo = new UtilisateurRepository(factory);

            await repo.AddUserAsync(new Utilisateur
            {
                FullName = "Jackie Chan",
                Username = "JackieChan67",
                Password = "Password123$$123//",
                Email = "jackiechan67@gmail.com"
            });

            var test = await repo.GetUtilisateurByIdAsync(1);
            Assert.NotNull(test);
        }

        [Fact]
        public async Task GetByUtilisateurIdAsync_UtilisateurAbsent_RetourneNull()
        {
            var factory = DbContextFactoryTest.Creer();
            var repo = new UtilisateurRepository(factory);
            var test = await repo.GetUtilisateurByIdAsync(666);
            Assert.Null(test);
        }

        [Fact]
        public async Task UpdateUserAsync_UtilisateurPresent_MiseAJourFaite()
        {
            var factory = DbContextFactoryTest.Creer();
            var repo = new UtilisateurRepository(factory);

            var user = new Utilisateur
            {
                FullName = "Jackie Chan",
                Username = "JackieChan67",
                Password = "Password123$$123//",
                Email = "jackiechan67@gmail.com"
            };
            await repo.AddUserAsync(user);

            user.FullName = "Prout Master";
            user.Username = "ProutMaster69";

            await repo.UpdateUserAsync(user);

            var test = await repo.GetByUsernameAsync("ProutMaster69");
            Assert.NotNull(test);
            Assert.Equal("Prout Master", test.FullName);
            
        }

        [Fact]
        public async Task UpdateUserAsync_UtilisateurAbsent_MiseAJourNonFaite()
        {
            var factory = DbContextFactoryTest.Creer();
            var repo = new UtilisateurRepository(factory);

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
            using var context = factory.CreateDbContext();
            var user = await repo.GetByUsernameAsync("JackieChan67");
            Assert.Single(context.Utilisateurs);
            Assert.Equal("Jackie Chan", user!.FullName);
        }

        [Fact]
        public async Task DeleteUserAsync_UtilisateurPresent_EstSupprimer()
        {
            var factory = DbContextFactoryTest.Creer();
            var repo = new UtilisateurRepository(factory);

            var user = new Utilisateur
            {
                FullName = "Jackie Chan",
                Username = "JackieChan67",
                Password = "Password123$$123//",
                Email = "jackiechan67@gmail.com"
            };

            await repo.AddUserAsync(user);

            await repo.DeleteUserAsync(user);
            using var context = factory.CreateDbContext();
            Assert.Empty(context.Utilisateurs);
        }

        [Fact]
        public async Task DeleteUserAsync_UtilisateurAbsent_SuppressionNonFaite()
        {
            var factory = DbContextFactoryTest.Creer();
            var repo = new UtilisateurRepository(factory);
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

            using var context = factory.CreateDbContext();
            Assert.Single(context.Utilisateurs);
        }

        [Fact]
        public async Task GetUtilisateursAsync_ListeAvecUtilisateurs_RetourneLaListeComplete()
        {
            var factory = DbContextFactoryTest.Creer();
            var repo = new UtilisateurRepository(factory);
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
            var factory = DbContextFactoryTest.Creer();
            var repo = new UtilisateurRepository(factory);
            var test = await repo.GetUtilisateursAsync();
            Assert.Empty(test);
        }

    }
}
