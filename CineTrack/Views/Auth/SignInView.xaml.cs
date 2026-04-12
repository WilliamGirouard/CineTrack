using CineTrack.ViewModels.Auth;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Controls;


namespace CineTrack.Views.Auth
{
    /// Interaction logic for SignInView.xaml
   
    public partial class SignInView : UserControl
    {
        public SignInView()
        {
            InitializeComponent();

            DataContext = App.ServiceProvider.GetRequiredService<SignInViewModel>();

            //pour re remplir le password box si remember me est coché
            if (DataContext is SignInViewModel valeurUser && valeurUser.RememberMe) {
                PasswordBox.Password = valeurUser.Password;
            }
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is SignInViewModel viewModel)
            {
                viewModel.Password = ((PasswordBox)sender).Password;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
