using System.Windows.Controls;
using System.Windows.Media.Animation;
using CineTrack.ViewModels.Watch;

namespace CineTrack.Views.Watch;

public partial class WatchView : UserControl
{
    public WatchView()
    {
        InitializeComponent();
        DataContextChanged += OnDataContextChanged;
    }

    private void OnDataContextChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
    {
        if (DataContext is WatchViewModel vm)
        {
            // Écoute les changements d'épisode pour jouer l'animation
            vm.PropertyChanged += (s, args) =>
            {
                if (args.PropertyName == nameof(vm.EpisodeNumber))
                    PlayEpisodeAnimation();
            };
        }
    }

    // Animation de fondu lors du changement d'épisode
    private void PlayEpisodeAnimation()
    {
        var fadeOut = new DoubleAnimation(1, 0, TimeSpan.FromMilliseconds(200));
        var fadeIn = new DoubleAnimation(0, 1, TimeSpan.FromMilliseconds(400));
        fadeOut.Completed += (s, e) => VideoImage.BeginAnimation(OpacityProperty, fadeIn);
        VideoImage.BeginAnimation(OpacityProperty, fadeOut);
    }
}
