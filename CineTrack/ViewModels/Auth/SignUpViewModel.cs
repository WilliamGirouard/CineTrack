using CineTrack.Data.Models;
using CineTrack.Data.Repositories.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CineTrack.ViewModels.Auth
{
    public partial class SignUpViewModel : ObservableObject
    {
        private readonly IUtilisateurRepository utilisateurRepository;

        [ObservableProperty]
        private string _username;
        [ObservableProperty]
        private string _firstName;
        [ObservableProperty]
        private string _lastName;
        [ObservableProperty]
        private string _email;
        [ObservableProperty]
        private string _password;
        [ObservableProperty]
        private string _error;

        public ICommand SignUpCommand { get; set; }
        public ICommand ToggleHiddenCommand { get; set; }

       public SignUpViewModel(IUtilisateurRepository utilisateurRepository)
        {
            this.utilisateurRepository = utilisateurRepository;

            //SignUpCommand = new RelayCommand(
            //    execute: () =>
            //    {
            //        utilisateurRepository.AddUser(user)
            //    });
            //ToggleHiddenCommand = new RelayCommand<bool>()
        }
    }
}
