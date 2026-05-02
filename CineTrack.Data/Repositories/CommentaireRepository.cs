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
        private readonly IDbContextFactory<CineTrackDbContext> _context;

        public CommentaireRepository(IDbContextFactory<CineTrackDbContext> context)
        {
            _context = context;
        }

        public async Task<Commentaire> AddCommentaireAsync(Commentaire commentaire)
        {
            using var context = _context.CreateDbContext();
            await context.Commentaires.AddAsync(commentaire);
            await context.SaveChangesAsync();
            return commentaire;
        }

        public async Task<List<Commentaire>> GetCommentairesByAnimeIdAsync(long malId)
        {
            using var context = _context.CreateDbContext();
            return await context.Commentaires
               .Where(c => c.MalId == malId)
               .Include(c => c.Utilisateur)
               .OrderByDescending(c => c.DateCreation)
               .ToListAsync();
        }

        public async Task RemoveCommentaireAsync(int commentaireId)
        {
            using var context = _context.CreateDbContext();
            var commentaire = await context.Commentaires.FindAsync(commentaireId);
            if (commentaire != null)
            {
                context.Commentaires.Remove(commentaire);
                await context.SaveChangesAsync();
            }
        }
        public async Task<Commentaire?> GetCommentaireAsync(int commentaireId)
        {
            using var context = _context.CreateDbContext();
            return await context.Commentaires.FindAsync(commentaireId);
        }
        public async Task<List<Commentaire>> GetCommentairesRecentAsync()
        {
            using var context = _context.CreateDbContext();
            return await context.Commentaires
                .OrderByDescending(c => c.DateCreation)
                .Take(10)
                .ToListAsync();
        }
    }
}
