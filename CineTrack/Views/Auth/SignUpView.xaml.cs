using CineTrack.ViewModels.Auth;
using Microsoft.Extensions.DependencyInjection;
using System.Windows.Controls;

namespace CineTrack.Views.Auth
{
    public partial class SignUpView : Page
    {
        public SignUpView()
        {
            InitializeComponent();
            // Assigner le ViewModel depuis le conteneur DI
            DataContext = App.ServiceProvider.GetRequiredService<SignUpViewModel>();
        }

        // WPF ne permet pas de binder PasswordBox.Password directement (sécurité).
        // On synchronise manuellement vers le ViewModel via PasswordChanged.
        private void PasswordBox_PasswordChanged(object sender, System.Windows.RoutedEventArgs e)
        {
            if (DataContext is SignUpViewModel vm)
                vm.Password = PasswordBox.Password;
        }

        private void ConfirmPasswordBox_PasswordChanged(object sender, System.Windows.RoutedEventArgs e)
        {
            if (DataContext is SignUpViewModel vm)
                vm.ConfirmPassword = ConfirmPasswordBox.Password;
        }
    }
}
