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
    /// Interaction logic for ForgottenPasswordView.xaml
    /// </summary>
    public partial class ForgottenPasswordView : UserControl
    {
        public ForgottenPasswordView()
        {
            InitializeComponent();
            DataContext = App.ServiceProvider.GetRequiredService<ForgottenPasswordViewModel>();
        }
    }
}
