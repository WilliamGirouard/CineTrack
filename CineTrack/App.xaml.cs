using CineTrack.Data.Context;
using CineTrack.Data.Repositories;
using CineTrack.Data.Repositories.Interfaces;
using CineTrack.Data.Services;
using CineTrack.Data.Services.Interfaces;
using CineTrack.Services;
using CineTrack.Services.Interfaces;
using CineTrack.ViewModels.Auth;
using CineTrack.Views.Auth;
using CineTrack.Services;
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
        //faut add un petit truc pour la navigation entre les pages
        
        public static IServiceProvider ServiceProvider { get; private set; } = null!;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var services = new ServiceCollection();

            var connectionString = configuration.GetConnectionString("DefaultConnection");
            var provider = configuration["DatabaseProvider"];

            if (provider == "SQLite")
                services.AddDbContext<CineTrackDbContext>(o => o.UseSqlite(connectionString));
            else if (provider == "SqlServer")
                services.AddDbContext<CineTrackDbContext>(o => o.UseSqlServer(connectionString));

            services.AddScoped<IUtilisateurRepository, UtilisateurRepository>();
            services.AddScoped<IUtilisateurService, UtilisateurService>();
            services.AddSingleton<INavigationService, NavigationService>();

            services.AddScoped<IUtilisateurAnimeRepository, UtilisateurAnimeRepository>();
            services.AddScoped<IUtilisateurAnimeService, UtilisateurAnimeService>();

            services.AddScoped<IAnimeRepository, AnimeRepository>();
            services.AddScoped<IAnimeService, AnimeService>();
            services.AddScoped<IAnimeApiService, AnimeApiService>();

            services.AddSingleton<INavigationService, NavigationService>();

            services.AddTransient<SignInViewModel>();
            services.AddTransient<SignUpViewModel>();   

            services.AddTransient<MainWindow>();
            services.AddTransient<SignUpView>();
            services.AddTransient<SignInView>();

            ServiceProvider = services.BuildServiceProvider();

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
