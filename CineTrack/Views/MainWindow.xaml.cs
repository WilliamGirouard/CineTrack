using CineTrack.Data.Services.Interfaces;
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
        }

        public void NavigateTo(System.Windows.Controls.Page page)
        {
            MainFrame.Navigate(page);
        }
    }
}