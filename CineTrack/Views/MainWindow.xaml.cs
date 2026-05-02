using CineTrack.Services.Interfaces;
using CineTrack.ViewModels.Auth;
using CineTrack.Views.Auth;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;

namespace CineTrack
{
    public partial class MainWindow : Window
    {
        public MainWindow(INavigationService navigationService)
        {
            InitializeComponent();
            DataContext = navigationService;
        }
    }
}