using CineTrack.ViewModels;
using CineTrack.ViewModels.Carousel;
using System.Windows;
using System.Windows.Controls;


namespace CineTrack.Views
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : UserControl
    {
        public MainView()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            if (DataContext is MainViewModel vm && vm.LoadCommand.CanExecute(null))
                vm.LoadCommand.Execute(null);
        }
        private void CardsPanel_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (sender is FrameworkElement el && el.DataContext is CarouselViewModel vm)
                vm.UpdateWidth(e.NewSize.Width);
        }
    }
}
