using CineTrack.Data.Context;
using CineTrack.Data.Repositories;
using CineTrack.Data.Repositories.Interfaces;
using CineTrack.Data.Services;
using CineTrack.Data.Services.Interfaces;
using CineTrack.Services;
using CineTrack.Services.Interfaces;
using CineTrack.Services.Jikan;
using CineTrack.ViewModels;
using CineTrack.ViewModels.AnimeDetails;
using CineTrack.ViewModels.Auth;
using CineTrack.ViewModels.Auth.PasswordReset;
using CineTrack.ViewModels.Favoris;
using CineTrack.ViewModels.Favoris;
using CineTrack.ViewModels.Search;
using CineTrack.Views.Auth;
using CineTrack.Views.Auth.PasswordReset;
using CineTrack.Views.Favoris;
using CineTrack.Views.Search;
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

            var emailConfiguration = configuration.GetSection("Email");

            var services = new ServiceCollection();

            // ── Base de données ────────────────────────────────────────────────
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            var provider = configuration["DatabaseProvider"];
            if (provider == "SQLite")
                services.AddDbContext<CineTrackDbContext>(o => o.UseSqlite(connectionString));
            else if (provider == "SqlServer")
                services.AddDbContext<CineTrackDbContext>(o => o.UseSqlServer(connectionString));
            // Email Service
            services.AddSingleton<IEmailService>(new EmailService(
                emailConfiguration["mail"]!,
                emailConfiguration["password"]!,
                emailConfiguration["SmtpHost"]!,
                int.Parse(emailConfiguration["SmtpPort"]!)
                ));

            // ── Repositories (Scoped) ──────────────────────────────────────────
            services.AddScoped<IUtilisateurRepository, UtilisateurRepository>();
            services.AddScoped<IUtilisateurService, UtilisateurService>();

            services.AddScoped<IUtilisateurAnimeRepository, UtilisateurAnimeRepository>();
            services.AddScoped<IUtilisateurAnimeService, UtilisateurAnimeService>();

            services.AddScoped<IAnimeService, AnimeService>();
            services.AddScoped<IAnimeRepository, AnimeRepository>();

            services.AddScoped<IFavorisRepository, FavorisRepository>();

            // ── Navigation (Singleton) ─────────────────────────────────────────
            services.AddSingleton<INavigationService, NavigationService>();
            services.AddSingleton<IJikanService, JikanService>();
            services.AddSingleton<MainViewModel>();

            //PasswordResetCodeStorage
            services.AddSingleton<PasswordResetStore>();
            // ── ViewModels (Transient) ─────────────────────────────────────────
            services.AddTransient<SignInViewModel>();
            services.AddTransient<SignUpViewModel>();
            services.AddTransient<FavorisViewModel>();
            services.AddTransient<AnimeDetailsViewModel>();
            services.AddTransient<ForgottenPasswordViewModel>();
            services.AddTransient<ResetCodeVerificationViewModel>();
            services.AddTransient<ResetPasswordViewModel>();

            // ── Views (Transient) ──────────────────────────────────────────────
            services.AddTransient<MainWindow>();
            services.AddTransient<SignUpView>();
            services.AddTransient<SignInView>();
            services.AddTransient<FavorisView>();
            services.AddTransient<ForgottenPasswordView>();
            services.AddTransient<ResetCodeVerificationView>();
            services.AddTransient<ResetPasswordView>();
            services.AddTransient<SearchViewModel>();
            services.AddTransient<SearchView>();

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