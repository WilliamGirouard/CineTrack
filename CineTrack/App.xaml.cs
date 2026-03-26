using CineTrack.Data.Context;
using CineTrack.Data.Repositories;
using CineTrack.Data.Repositories.Interfaces;
using CineTrack.Data.Services;
using CineTrack.Data.Services.Interfaces;
using CineTrack.ViewModels.Auth;
using CineTrack.Views.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using System.Windows;

namespace CineTrack
{
    public partial class App : Application
    {
        public static IServiceProvider ServiceProvider { get; private set; } = null!;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var services = new ServiceCollection();

            // ── Base de données ────────────────────────────────────────────────
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            var provider = configuration["DatabaseProvider"];

            if (provider == "SQLite")
                services.AddDbContext<CineTrackDbContext>(o => o.UseSqlite(connectionString));
            else if (provider == "SqlServer")
                services.AddDbContext<CineTrackDbContext>(o => o.UseSqlServer(connectionString));

            // ── Repositories (Scoped) ──────────────────────────────────────────
            services.AddScoped<IUtilisateurRepository, UtilisateurRepository>();

            // ── Services métier (Scoped) ───────────────────────────────────────
            services.AddScoped<IUtilisateurService, UtilisateurService>();

            // ── ViewModels (Transient) ─────────────────────────────────────────
            services.AddTransient<SignInViewModel>();
            services.AddTransient<SignUpViewModel>();   // injecte IUtilisateurService

            // ── Views (Transient) ──────────────────────────────────────────────
            services.AddTransient<MainWindow>();
            services.AddTransient<SignUpView>();
            services.AddTransient<SignInView>();

            ServiceProvider = services.BuildServiceProvider();

            // Appliquer les migrations au démarrage
            using (var scope = ServiceProvider.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<CineTrackDbContext>();
                db.Database.Migrate();
            }

            var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            if (ServiceProvider is IDisposable disposable)
                disposable.Dispose();
            base.OnExit(e);
        }
    }
}