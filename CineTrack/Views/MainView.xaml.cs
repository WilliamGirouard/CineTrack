using CineTrack.ViewModels;
using CineTrack.ViewModels.Carousel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;


namespace CineTrack.Views
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : UserControl
    {
        private static double _savedScrollOffset = 0;

        public MainView()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            if (DataContext is MainViewModel vm && vm.LoadCommand.CanExecute(null))
                vm.LoadCommand.Execute(null);

            // Restore scroll position after layout is ready
            LandingPage.Dispatcher.BeginInvoke(() =>
            {
                LandingPage.ScrollToVerticalOffset(_savedScrollOffset);
            }, System.Windows.Threading.DispatcherPriority.Loaded);
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            // Save scroll position before leaving
            _savedScrollOffset = LandingPage.VerticalOffset;
        }

        private void CardsPanel_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (sender is FrameworkElement el && el.DataContext is CarouselViewModel vm)
                vm.UpdateWidth(e.NewSize.Width);
        }

        private void OnPreviewMouseWheel(object sender, MouseWheelEventArgs e) // this prevents the scolling from stopping when your mouse is hovering over an anime card
    {                                                                          // source: https://stackoverflow.com/questions/9019304/the-mouse-wheel-event-doesnt-work-correcty-on-a-lisbox-when-it-has-scrollviewer
            e.Handled = true;

            var e2 = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta);
            e2.RoutedEvent = ListBox.MouseWheelEvent;
            e2.Source = e.Source;

            LandingPage.RaiseEvent(e2);
        }
    }
}
