namespace CineTrack.ViewModels.Watch;

// Paramètre de navigation vers la page de visionnage d'un épisode
public class WatchNavParam
{
    public long MalId { get; set; }
    public int EpisodeNumber { get; set; }
    public string Source { get; set; } = "main";
}