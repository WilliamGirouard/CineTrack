using CineTrack.Services;
using CineTrack.ViewModels.Auth;
using CineTrack.Views.Auth;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace CineTrack
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            // Afficher SignUpView au démarrage
            // (plus tard ce sera SignInView, ou une logique qui vérifie la session)
            var signUpView = App.ServiceProvider.GetRequiredService<SignUpView>();
            var signInView = App.ServiceProvider.GetService<SignInView>();
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
        /// Permet aux ViewModels de naviguer entre les pages.
        /// Appelé depuis le code-behind ou via un service de navigation.
        public void NavigateTo(System.Windows.Controls.Page page)
        {
            MainFrame.Navigate(page);
        }
    }
}
