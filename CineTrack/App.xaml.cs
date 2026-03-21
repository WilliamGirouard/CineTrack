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
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IServiceProvider ServiceProvider { get; private set; }
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
            // Configuration selon le provider (https://cegepmv.github.io/420-413/efcore/index.html)
            // ═══════════════════════════════════════════
            // 2. BASE DE DONNÉES (Scoped)
            // ═══════════════════════════════════════════
            if (provider == "SQLite")
            {
                services.AddDbContext<CineTrackDbContext>(options =>
                    options.UseSqlite(connectionString));
            }
            else if (provider == "SqlServer")
            {
                services.AddDbContext<CineTrackDbContext>(options =>
                    options.UseSqlServer(connectionString));
            }

            //Conteneur Inversion of Control https://cegepmv.github.io/420-413/injection_dependance/index.html

            // ═══════════════════════════════════════════
            // 3. REPOSITORIES (Scoped)
            // ═══════════════════════════════════════════
            services.AddScoped<IUtilisateurRepository, UtilisateurRepository>();

            // ═══════════════════════════════════════════
            // 4. SERVICES MÉTIER (Scoped)
            // ═══════════════════════════════════════════
            services.AddScoped<IUtilisateurService, UtilisateurService>();

            // ═══════════════════════════════════════════
            // 5. VIEWMODELS (Transient)
            // ═══════════════════════════════════════════
            services.AddTransient<SignInViewModel>();
            services.AddTransient<SignUpViewModel>();

            // ═══════════════════════════════════════════
            // 6. VIEWS (Transient)
            // ═══════════════════════════════════════════
            services.AddTransient<MainWindow>();
            services.AddTransient<SignUpView>();
            services.AddTransient<SignInView>();

            // ═══════════════════════════════════════════
            // 7. SINGLETONS (ex. : Logger)
            // ═══════════════════════════════════════════
            // services.AddSingleton<ILogger, FileLogger>();

            // 8. Construire le provider et l'exposer
            // On construit le "conteneur" (celui qui fabrique les objets)
            ServiceProvider = services.BuildServiceProvider();

            // 9. Résoudre et afficher MainWindow
            var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }
        protected override void OnExit(ExitEventArgs e)
        {
            // Libérer les ressources si nécessaire
            if (ServiceProvider is IDisposable disposable)
                disposable.Dispose();

            base.OnExit(e);
        }
    }
}
