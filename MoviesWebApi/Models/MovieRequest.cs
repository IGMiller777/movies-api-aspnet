namespace MoviesWebApi.Models;

public class MovieRequest
{
    public string? Title { get; set; }

    public string? Genre { get; set; }

    public string ReleaseDate { get; set; } = string.Empty;
}