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
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is SignInViewModel viewModel)
            {
                viewModel.Password = ((PasswordBox)sender).Password;
            }
        }
    }
}
