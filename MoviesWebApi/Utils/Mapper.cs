using MoviesWebApi.Database;
using MoviesWebApi.Models;

namespace MoviesWebApi.Utils;

public static class Mapper
{
    public static Movie Convert(MovieRequest movieRequest)
    {
        return new Movie(){ Genre = movieRequest.Genre, Title = movieRequest.Title, ReleaseDate = movieRequest.ReleaseDate};
    }
    
    public static MovieResponse Convert(Movie movie)
    {
        return new MovieResponse(){ Id = movie.Id, Genre = movie.Genre, Title = movie.Title, ReleaseDate = movie.ReleaseDate};
    }
    
    public static IEnumerable<MovieResponse> Convert(IEnumerable<Movie> movies)
    {
        return movies.Select(a => Convert(a));
    }
}