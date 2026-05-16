using CineTrack.Data.Context;
using CineTrack.Data.Repositories;
using CineTrack.Data.Repositories.Interfaces;
using CineTrack.Data.Services.AdminServ;
using CineTrack.Data.Services.EmailServ;
using CineTrack.Data.Services.NoteServ;
using CineTrack.Data.Services.PasswordResetStoreServ;
using CineTrack.Data.Services.UtilisateurServ;
using CineTrack.Services;
using CineTrack.Data.Services.NavigationServ;
using CineTrack.Services.Jikan;
using CineTrack.ViewModels;
using CineTrack.ViewModels.AnimeDetails;
using CineTrack.ViewModels.Auth;
using CineTrack.ViewModels.Auth.PasswordReset;
using CineTrack.ViewModels.Auth.Verification;
using CineTrack.ViewModels.Profile;
using CineTrack.ViewModels.Search;
using CineTrack.ViewModels.Settings;
using CineTrack.ViewModels.Watch;
using CineTrack.Views.Auth;
using CineTrack.Views.Auth.PasswordReset;
using CineTrack.Views.Auth.Verification;
using CineTrack.Views.Profile;
using CineTrack.Views.Search;
using CineTrack.Views.Settings;
using CineTrack.Views.Watch;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using System.Windows;
using CineTrack.ViewModels.Favoris;

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
                services.AddDbContextFactory<CineTrackDbContext>(o => o.UseSqlite(connectionString));
            else if (provider == "SqlServer")
                services.AddDbContextFactory<CineTrackDbContext>(o => o.UseSqlServer(connectionString));

            // Email Service
            services.AddSingleton<IEmailService>(new EmailService(
                emailConfiguration["mail"]!,
                emailConfiguration["password"]!,
                emailConfiguration["SmtpHost"]!,
                int.Parse(emailConfiguration["SmtpPort"]!)
                ));

            // Repositories
            services.AddScoped<IUtilisateurRepository, UtilisateurRepository>();
            services.AddScoped<IFavorisRepository, FavorisRepository>();
            services.AddScoped<ICommentaireRepository, CommentaireRepository>();
            services.AddScoped<INoteRepository, NoteRepository>();

            // Services
            services.AddScoped<IUtilisateurService, UtilisateurService>();
            services.AddScoped<INoteService, NoteService>();
            services.AddScoped<IAdminService, AdminService>();

            // Navigation
            services.AddSingleton<INavigationService, NavigationService>();
            services.AddSingleton<IJikanService, JikanService>();
            services.AddSingleton<MainViewModel>();
            services.AddSingleton<WatchViewModel>();

            // PasswordResetCodeStorage
            services.AddSingleton<PasswordResetStore>();

            // ViewModels
            services.AddTransient<SignInViewModel>();
            services.AddTransient<SignUpViewModel>();
            services.AddTransient<AnimeDetailsViewModel>();
            services.AddTransient<ForgottenPasswordViewModel>();
            services.AddTransient<ResetCodeVerificationViewModel>();
            services.AddTransient<ResetPasswordViewModel>();
            services.AddTransient<SearchViewModel>();
            services.AddTransient<ProfileViewModel>();
            services.AddTransient<FavorisViewModel>();
            services.AddTransient<SettingsViewModel>();
            services.AddTransient<WatchView>();
            services.AddTransient<EmailVerificationViewModel>();

            // Views (Transient)
            services.AddTransient<MainWindow>();
            services.AddTransient<SignUpView>();
            services.AddTransient<SignInView>();
            services.AddTransient<ForgottenPasswordView>();
            services.AddTransient<ResetCodeVerificationView>();
            services.AddTransient<ResetPasswordView>();
            services.AddTransient<SearchView>();
            services.AddTransient<WatchView>();
            services.AddTransient<ProfileView>();
            services.AddTransient<SettingsView>();
            services.AddTransient<EmailVerificationView>();

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
