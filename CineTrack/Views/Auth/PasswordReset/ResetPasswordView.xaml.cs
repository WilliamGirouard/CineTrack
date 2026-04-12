using CineTrack.ViewModels.Auth;
using CineTrack.ViewModels.Auth.PasswordReset;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CineTrack.Views.Auth.PasswordReset
{
    /// <summary>
    /// Interaction logic for ResetPasswordView.xaml
    /// </summary>
    public partial class ResetPasswordView : UserControl
    {
        public ResetPasswordView()
        {
            InitializeComponent();
        }
        //Repris de SignUpView
        private void PasswordBox_PasswordChanged(object sender, System.Windows.RoutedEventArgs e)
        {
            if (DataContext is ResetPasswordViewModel vm)
                vm.NewPassword = PasswordBox.Password;
        }

        private void ConfirmPasswordBox_PasswordChanged(object sender, System.Windows.RoutedEventArgs e)
        {
            if (DataContext is ResetPasswordViewModel vm)
                vm.ConfirmPassword = ConfirmPasswordBox.Password;
        }
    }
}
