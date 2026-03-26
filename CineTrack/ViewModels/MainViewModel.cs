using CineTrack.Services;
using CineTrack.Session;
using CineTrack.ViewModels.Auth;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CineTrack.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly INavigationService _navigationService;

        public MainViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        [RelayCommand]
        private void Disconnect()
        {
            SessionManager.Instance.CloseSession();
            _navigationService.NavigateTo<SignInViewModel>();
        }
    }
}
