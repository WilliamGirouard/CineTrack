using CineTrack.Data.Models;

namespace CineTrack.Tests.Modeles
{
    public class FavorisTests
    {
        [Fact]
        public void Constructeur_SansParametres_IdEstZeroParDefaut()
        {
            var favoris = new Favoris();

            Assert.Equal(0, favoris.Id);
        }

        [Fact]
        public void Proprietes_AssignationDirecte_ValeursConservees()
        {
            var favoris = new Favoris
            {
                UtilisateurId = 1,
                MalId = 21
            };

            Assert.Equal(1, favoris.UtilisateurId);
            Assert.Equal(21, favoris.MalId);
        }

        [Theory]
        [InlineData(1, 21)]
        [InlineData(2, 1535)]
        [InlineData(99, 9999)]
        public void Proprietes_DiversUtilisateursEtAnimes_BienAssignes(int utilisateurId, long malId)
        {
            var favoris = new Favoris
            {
                UtilisateurId = utilisateurId,
                MalId = malId
            };

            Assert.Equal(utilisateurId, favoris.UtilisateurId);
            Assert.Equal(malId, favoris.MalId);
        }
    }
}