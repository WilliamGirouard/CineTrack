using CineTrack.Data.Models;

namespace CineTrack.Tests.Fakes
{
    public class FakeNoteRepositoryTests
    {
        [Fact]
        public async Task AddNoteAsync_NouvelleNote_EstAjouteListe()
        {
            var repo = new FakeNoteRepository();
            var note = new Note { UtilisateurId = 1, MalId = 100, NoteUtilisateur = 4 };

            await repo.AddNoteAsync(note);

            Assert.Single(repo.Notes);
            Assert.Equal(4, repo.Notes[0].NoteUtilisateur);
        }

        [Fact]
        public async Task GetNoteByUserIdAndAnimeIdAsync_NotePresente_RetourneNote()
        {
            var repo = new FakeNoteRepository();
            await repo.AddNoteAsync(new Note { UtilisateurId = 1, MalId = 100, NoteUtilisateur = 3 });

            var result = await repo.GetNoteByUserIdAndAnimeIdAsync(1, 100);

            Assert.NotNull(result);
            Assert.Equal(3, result.NoteUtilisateur);
        }

        [Fact]
        public async Task GetNoteByUserIdAndAnimeIdAsync_NoteAbsente_RetourneNull()
        {
            var repo = new FakeNoteRepository();

            var result = await repo.GetNoteByUserIdAndAnimeIdAsync(99, 999);

            Assert.Null(result);
        }

        [Fact]
        public async Task GetNotesByAnimeIdAsync_PlusieursNotes_RetourneListeFiltree()
        {
            var repo = new FakeNoteRepository();
            await repo.AddNoteAsync(new Note { UtilisateurId = 1, MalId = 100, NoteUtilisateur = 2 });
            await repo.AddNoteAsync(new Note { UtilisateurId = 2, MalId = 100, NoteUtilisateur = 5 });
            await repo.AddNoteAsync(new Note { UtilisateurId = 3, MalId = 999, NoteUtilisateur = 1 });

            var result = await repo.GetNotesByAnimeIdAsync(100);

            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task GetNotesByAnimeIdAsync_AucuneNote_RetourneListeVide()
        {
            var repo = new FakeNoteRepository();

            var result = await repo.GetNotesByAnimeIdAsync(100);

            Assert.Empty(result);
        }

        [Fact]
        public async Task UpdateNoteAsync_NotePresente_ValeurMiseAJour()
        {
            var repo = new FakeNoteRepository();
            await repo.AddNoteAsync(new Note { UtilisateurId = 1, MalId = 100, NoteUtilisateur = 2 });

            var note = repo.Notes[0];
            note.NoteUtilisateur = 5;
            await repo.UpdateNoteAsync(note);

            Assert.Equal(5, repo.Notes[0].NoteUtilisateur);
        }

        [Fact]
        public async Task UpdateNoteAsync_NoteAbsente_AucuneModification()
        {
            var repo = new FakeNoteRepository();
            await repo.AddNoteAsync(new Note { UtilisateurId = 1, MalId = 100, NoteUtilisateur = 2 });

            var noteAbsente = new Note { Id = 666, UtilisateurId = 99, MalId = 999, NoteUtilisateur = 5 };
            await repo.UpdateNoteAsync(noteAbsente);

            Assert.Equal(2, repo.Notes[0].NoteUtilisateur);
        }
    }
}