namespace CineTrack.ViewModels.Profile;

public class ProfileNavParam
{
    public int UserId { get; set; }

    public string Source { get; set; } = "main";

    // if source is "animeDetails", we specify which anime
    public long? SourceMalId { get; set; }
}
