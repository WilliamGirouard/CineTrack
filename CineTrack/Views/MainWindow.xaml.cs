<<<<<<< HEAD
using CineTrack.Services;
using CineTrack.ViewModels.Auth;
=======
using CineTrack.Data.Services.Interfaces;
>>>>>>> 703482772dbb4fce2ccf0f62bba2ef19b82c61e4
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

            // Show SignUpView for navigation test
            var signUpView = App.ServiceProvider.GetRequiredService<SignUpView>();
            MainFrame.Navigate(signUpView);

            var nav = (NavigationService)App.ServiceProvider.GetRequiredService<INavigationService>();
            nav.PropertyChanged += (s, e) =>
            {
                if (nav.CurrentView is SignInViewModel) //logique if pour inverse les page de sign IN and UP 
                    MainFrame.Navigate(signInView);
                else if (nav.CurrentView is SignUpViewModel)
                    MainFrame.Navigate(signUpView);
            };
        }

        public void NavigateTo(System.Windows.Controls.Page page)
        {
            MainFrame.Navigate(page);
        }
    }
}