namespace MoviesWebApi.Models
{
    public class Movie
    {
        public int Id { get; set; }

        public string? Title { get; set; }

        public string? Genre { get; set; }

        public string ReleaseDate { get; set; } = string.Empty;
    }
}
