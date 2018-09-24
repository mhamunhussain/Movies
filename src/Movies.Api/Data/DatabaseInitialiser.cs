using System;
using System.Linq;

namespace Movies.Api.Data
{
    public class DatabaseInitialiser : IDatabaseInitialiser
    {
        private readonly ApplicationDbContext _context;

        public DatabaseInitialiser(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Initialise()
        {
            _context.Database.EnsureCreated();

            var movieId1 = Guid.NewGuid();
            var movieId2 = Guid.NewGuid();
            var movieId3 = Guid.NewGuid();
            var userId = Guid.NewGuid();

            if (!_context.Movies.Any())
            {
                _context.Movies.AddRange(
                    new Movie
                    {
                        Id = movieId1,
                        Title = "Donnie Darko",
                        YearOfRelease = 2001,
                        RunningTime = 90,
                        Genres = "Thriller"
                    },
                    new Movie
                    {
                        Id = movieId2,
                        Title = "Get Out",
                        YearOfRelease = 2017,
                        RunningTime = 90,
                        Genres = "Comedy, Thriller"
                    },
                    new Movie
                    {
                        Id = movieId3,
                        Title = "Goonies",
                        YearOfRelease = 1985,
                        RunningTime = 90,
                        Genres = "Comedy"
                    }
                );
            }

            if (!_context.Users.Any())
            {
                _context.Users.AddRange(
                    new User { Id = userId, FullName = "John Doe" }
                );
            }

            if (!_context.MovieRatings.Any())
            {
                _context.MovieRatings.AddRange(
                    new MovieRating { Id = Guid.NewGuid(), MovieId = movieId1, Rating = 8, UserId = userId },
                    new MovieRating { Id = Guid.NewGuid(), MovieId = movieId2, Rating = 8.5, UserId = userId }
                );
            }

            _context.SaveChanges();
        }
    }
}
