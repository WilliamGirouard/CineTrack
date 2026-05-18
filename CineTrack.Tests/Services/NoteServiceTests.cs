using CineTrack.Data.Models;
using CineTrack.Data.Repositories.Interfaces;
using CineTrack.Data.Services.NoteServ;
using Moq;

namespace CineTrack.Tests.Services
{
    public class NoteServiceTests
    {
        private readonly Mock<INoteRepository> _mockRepo;
        private readonly NoteService _service;

        public NoteServiceTests()
        {
            _mockRepo = new Mock<INoteRepository>();
            _service = new NoteService(_mockRepo.Object);
        }

        // Test 1: Si aucune note existante, AddNoteAsync est appelé
        [Fact]
        public async Task RateAsync_PasDeNoteExistante_AppelleAddNote()
        {
            // Arrange: GetNote retourne null (pas encore de note)
            _mockRepo
                .Setup(r => r.GetNoteByUserIdAndAnimeIdAsync(It.IsAny<int>(), It.IsAny<long>()))
                .ReturnsAsync((Note?)null);

            // Act
            await _service.RateAsync(utilisateurId: 1, malId: 100, note: 4);

            // Assert: AddNoteAsync appelé une fois
            _mockRepo.Verify(r => r.AddNoteAsync(It.Is<Note>(n =>
                n.UtilisateurId == 1 &&
                n.MalId == 100 &&
                n.NoteUtilisateur == 4
            )), Times.Once);
        }

        // Test 2: Si une note existe déjà, UpdateNoteAsync est appelé (pas Add)
        [Fact]
        public async Task RateAsync_NoteExistante_AppelleUpdateNote()
        {
            // Arrange: il y a déjà une note
            var noteExistante = new Note { Id = 5, UtilisateurId = 1, MalId = 100, NoteUtilisateur = 3 };
            _mockRepo
                .Setup(r => r.GetNoteByUserIdAndAnimeIdAsync(1, 100))
                .ReturnsAsync(noteExistante);

            // Act
            await _service.RateAsync(utilisateurId: 1, malId: 100, note: 5);

            // Assert: UpdateNoteAsync appelé, AddNoteAsync jamais appelé
            _mockRepo.Verify(r => r.UpdateNoteAsync(It.IsAny<Note>()), Times.Once);
            _mockRepo.Verify(r => r.AddNoteAsync(It.IsAny<Note>()), Times.Never);
        }

        // Test 3: GetCommunityScoreAsync retourne null si aucune note
        [Fact]
        public async Task GetCommunityScoreAsync_AucuneNote_RetourneNull()
        {
            // Arrange
            _mockRepo
                .Setup(r => r.GetNotesByAnimeIdAsync(It.IsAny<long>()))
                .ReturnsAsync(new List<Note>());

            // Act
            var result = await _service.GetCommunityScoreAsync(malId: 100);

            // Assert
            Assert.Null(result);
        }

        // Test 4: GetCommunityScoreAsync calcule la bonne moyenne
        [Fact]
        public async Task GetCommunityScoreAsync_PlusieursNotes_RetourneMoyenne()
        {
            // Arrange: 3 notes de valeurs 2, 4, 3 → moyenne = 3.0
            var notes = new List<Note>
            {
                new Note { NoteUtilisateur = 2 },
                new Note { NoteUtilisateur = 4 },
                new Note { NoteUtilisateur = 3 }
            };
            _mockRepo
                .Setup(r => r.GetNotesByAnimeIdAsync(100))
                .ReturnsAsync(notes);

            // Act
            var result = await _service.GetCommunityScoreAsync(malId: 100);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3.0, result.Value, precision: 1);
        }

        // Test 5: La note existante est bien mise à jour avec la nouvelle valeur
        [Fact]
        public async Task RateAsync_NoteExistante_ValeurMiseAJour()
        {
            // Arrange
            var noteExistante = new Note { Id = 5, UtilisateurId = 1, MalId = 100, NoteUtilisateur = 2 };
            _mockRepo
                .Setup(r => r.GetNoteByUserIdAndAnimeIdAsync(1, 100))
                .ReturnsAsync(noteExistante);

            Note? noteMiseAJour = null;
            _mockRepo
                .Setup(r => r.UpdateNoteAsync(It.IsAny<Note>()))
                .Callback<Note>(n => noteMiseAJour = n);

            // Act
            await _service.RateAsync(utilisateurId: 1, malId: 100, note: 5);

            // Assert: la note passée à UpdateNoteAsync a bien la nouvelle valeur
            Assert.NotNull(noteMiseAJour);
            Assert.Equal(5, noteMiseAJour!.NoteUtilisateur);
        }
    }
}