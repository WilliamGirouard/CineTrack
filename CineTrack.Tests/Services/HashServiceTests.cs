using CineTrack.Data.Services.HashServ;

namespace CineTrack.Tests.Services
{
    public class HashServiceTests
    {
        // Le hash retourné n'est pas null ou vide
        [Fact]
        public void PasswordHasher_PasswordValide_RetourneHashNonVide()
        {
            var hash = HashService.PasswordHasher("MonMotDePasse123!");

            Assert.NotNull(hash);
            Assert.NotEmpty(hash);
        }

        // Le hash est différent du mot de passe original
        [Fact]
        public void PasswordHasher_PasswordValide_HashDifferentDuMotDePasse()
        {
            var password = "MonMotDePasse123!";
            var hash = HashService.PasswordHasher(password);

            Assert.NotEqual(password, hash);
        }

        //  Deux hash du même mot de passe sont différents (BCrypt salt aléatoire)
        [Fact]
        public void PasswordHasher_MemePassword_ProduitsDeuxHashDifferents()
        {
            var password = "MonMotDePasse123!";

            var hash1 = HashService.PasswordHasher(password);
            var hash2 = HashService.PasswordHasher(password);

            Assert.NotEqual(hash1, hash2);
        }

        // Vérification réussie avec le bon mot de passe
        [Fact]
        public void CompareHashToPassword_BonMotDePasse_RetourneTrue()
        {
            var password = "MonMotDePasse123!";
            var hash = HashService.PasswordHasher(password);

            var result = HashService.CompareHashToPassword(password, hash);

            Assert.True(result);
        }

        // Vérification échoue avec un mauvais mot de passe
        [Fact]
        public void CompareHashToPassword_MauvaisMotDePasse_RetourneFalse()
        {
            var hash = HashService.PasswordHasher("MonMotDePasse123!");

            var result = HashService.CompareHashToPassword("MauvaisPassword!", hash);

            Assert.False(result);
        }

        // Vérification échoue si le mot de passe est vide
        [Fact]
        public void CompareHashToPassword_PasswordVide_RetourneFalse()
        {
            var hash = HashService.PasswordHasher("MonMotDePasse123!");

            var result = HashService.CompareHashToPassword("", hash);

            Assert.False(result);
        }

        // Le hash commence par $2 (format BCrypt)
        [Fact]
        public void PasswordHasher_PasswordValide_HashFormatBCrypt()
        {
            var hash = HashService.PasswordHasher("MonMotDePasse123!");

            Assert.StartsWith("$2", hash);
        }
    }
}