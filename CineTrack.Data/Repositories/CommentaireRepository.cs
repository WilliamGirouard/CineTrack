using CineTrack.Data.Context;
using CineTrack.Data.Models;
using CineTrack.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CineTrack.Data.Repositories
{
    public class CommentaireRepository : ICommentaireRepository
    {
        private readonly CineTrackDbContext _context;

        public CommentaireRepository(CineTrackDbContext context)
        {
            _context = context;
        }

        public async Task<Commentaire> AddCommentaireAsync(Commentaire commentaire)
        {
            _context.Commentaires.Add(commentaire);
            await _context.SaveChangesAsync();
            return commentaire;
        }

        public async Task<List<Commentaire>> GetCommentairesByAnimeIdAsync(long malId)
        {
            return await _context.Commentaires
               .Where(c => c.MalId == malId)
               .Include(c => c.Utilisateur)
               .OrderByDescending(c => c.DateCreation)
               .ToListAsync();
        }

        public async Task RemoveCommentaireAsync(int commentaireId)
        {
            var commentaire = await _context.Commentaires.FindAsync(commentaireId);
            if (commentaire != null)
            {
                _context.Commentaires.Remove(commentaire);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<Commentaire?> GetCommentaireAsync(int commentaireId)
        {
            return await _context.Commentaires.FindAsync(commentaireId);
        }
    }
}
