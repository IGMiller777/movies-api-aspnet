﻿using Microsoft.EntityFrameworkCore;

namespace MoviesWebApi.Database
{
    public class MovieContext : DbContext
    {
        public MovieContext(DbContextOptions<MovieContext> options) : base(options) { }

        public DbSet<Movie> Movies { get; set; } = null;
    }
}
