using CineTrack.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CineTrack.Data.Context
{
    public class CineTrackDbContext : DbContext
    {
        public CineTrackDbContext(DbContextOptions<CineTrackDbContext> options) : base(options)
        {}

        public DbSet<Utilisateur> Utilisateurs { get; set; }
        public DbSet<Anime> Animes { get; set; }

        public DbSet<UtilisateurAnime> UtilisateurAnimes { get; set; }
        public DbSet<Favoris> Favoris { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Les garçons c'est ici qu'il y a l'unicité email et username
            modelBuilder.Entity<Utilisateur>(entity =>
            {
                entity.HasIndex(u => u.Email)
                .IsUnique();
                entity.HasIndex(u => u.Username).IsUnique();
            });

            modelBuilder.Entity<UtilisateurAnime>(entity =>
            {
                entity.HasKey(ua => new { ua.UtilisateurId, ua.MalId});
            });

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured) {
                optionsBuilder.UseSqlite("Data Source=CineTrack.db");            
            }
        }
    }
}
