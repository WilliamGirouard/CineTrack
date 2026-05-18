using CineTrack.Data.Models;
using CineTrack.Data.Repositories.Interfaces;

namespace CineTrack.Tests.Fakes
{
	public class FakeNoteRepository : INoteRepository
	{
		public List<Note> Notes { get; } = new List<Note>();

		public Task AddNoteAsync(Note note)
		{
			note.Id = Notes.Count + 1;
			Notes.Add(note);
			return Task.CompletedTask;
		}

		public Task<Note?> GetNoteByUserIdAndAnimeIdAsync(int userId, long animeId)
			=> Task.FromResult(Notes.FirstOrDefault(n => n.UtilisateurId == userId && n.MalId == animeId));

		public Task<List<Note>> GetNotesByAnimeIdAsync(long animeId)
			=> Task.FromResult(Notes.Where(n => n.MalId == animeId).ToList());

		public Task UpdateNoteAsync(Note note)
		{
			var existing = Notes.FirstOrDefault(n => n.Id == note.Id);
			if (existing != null)
				existing.NoteUtilisateur = note.NoteUtilisateur;
			return Task.CompletedTask;
		}
	}
}