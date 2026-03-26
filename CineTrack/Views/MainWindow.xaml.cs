using CineTrack.Data.Services.Interfaces;
using CineTrack.Services;
using CineTrack.ViewModels.Auth;
using CineTrack.Views.Auth;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;

namespace CineTrack
{
    public partial class MainWindow : Window
    {
        private readonly IUtilisateurService _utilisateurService;
        private readonly IAnimeService _animeService;
        private readonly IUtilisateurAnimeService _utilisateurAnimeService;

        public MainWindow(IUtilisateurService utilisateurService, IAnimeService animeService, IUtilisateurAnimeService utilisateurAnimeService)
        {
            InitializeComponent();
            _utilisateurService = utilisateurService;
            _animeService = animeService;
            _utilisateurAnimeService = utilisateurAnimeService;

            // Préparer les vues
            var signUpView = App.ServiceProvider.GetRequiredService<SignUpView>();
            var signInView = App.ServiceProvider.GetRequiredService<SignInView>();

            // Navigation entre SignIn et SignUp
            var nav = (NavigationService)App.ServiceProvider.GetRequiredService<INavigationService>();
            nav.PropertyChanged += (s, e) =>
            {
                if (nav.CurrentView is SignUpViewModel) //logique if pour inverser les pages SignIn et SignUp
                    MainFrame.Navigate(signUpView);
                else if (nav.CurrentView is SignInViewModel)
                    MainFrame.Navigate(signInView);
            };
        }

        public void NavigateTo(System.Windows.Controls.Page page)
        {
            MainFrame.Navigate(page);
        }

        //ajout du boutton login
        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            // Cacher la landing page et montrer le frame
            LandingPage.Visibility = Visibility.Collapsed;
            MainFrame.Visibility = Visibility.Visible;
            var signInView = App.ServiceProvider.GetRequiredService<SignInView>();
            MainFrame.Navigate(signInView);
        }
    }
}