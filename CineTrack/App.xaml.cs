using CineTrack.Data.Context;
using CineTrack.Data.Repositories;
using CineTrack.Data.Repositories.Interfaces;
using CineTrack.Data.Services;
using CineTrack.Data.Services.Interfaces;
using CineTrack.Services;
using CineTrack.Services.Interfaces;
using CineTrack.ViewModels;
using CineTrack.ViewModels.Auth;
using CineTrack.ViewModels.Favoris;
using CineTrack.Views.Auth;
using CineTrack.Views.Favoris;
using JikanDotNet;
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
            services.AddScoped<IUtilisateurService, UtilisateurService>();

            services.AddScoped<IUtilisateurAnimeRepository, UtilisateurAnimeRepository>();
            services.AddScoped<IUtilisateurAnimeService, UtilisateurAnimeService>();

            services.AddScoped<IAnimeService, AnimeService>();
            services.AddScoped<IAnimeApiService, AnimeApiService>();
            services.AddScoped<IAnimeRepository, AnimeRepository>();

            services.AddScoped<IFavorisRepository, FavorisRepository>();

            // ── Navigation (Singleton) ─────────────────────────────────────────
            services.AddSingleton<INavigationService, NavigationService>();

            // ── ViewModels (Transient) ─────────────────────────────────────────
            services.AddTransient<SignInViewModel>();
            services.AddTransient<SignUpViewModel>();
            services.AddTransient<MainViewModel>();
            services.AddTransient<FavorisViewModel>();

            // ── Views (Transient) ──────────────────────────────────────────────
            services.AddTransient<MainWindow>();
            services.AddTransient<SignUpView>();
            services.AddTransient<SignInView>();
            services.AddTransient<FavorisView>();

            ServiceProvider = services.BuildServiceProvider();

            // Appliquer les migrations au démarrage
            using (var scope = ServiceProvider.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<CineTrackDbContext>();
                db.Database.Migrate();
            }

            var navigationService = ServiceProvider.GetRequiredService<INavigationService>();
            navigationService.NavigateTo<SignInViewModel>();

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