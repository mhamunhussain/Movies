using Microsoft.EntityFrameworkCore;
using Movies.Api.Data;
using Movies.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Movies.Api.Repositories
{
    public class MovieRepository : IMovieRepository
    {
        private readonly ApplicationDbContext _context;

        public MovieRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<MovieDomainModel> GetAsync(Guid movieId)
        {
            var movie = await _context.Movies
                .Where(m => m.Id == movieId).SingleOrDefaultAsync();

            if (movie == null)
                return null;

            return new MovieDomainModel
            {
                Id = movie.Id,
                Title = movie.Title,
                YearOfRelease = movie.YearOfRelease,
                RunningTime = movie.RunningTime,
                AverageRating = _context.MovieRatings.Any(r => r.MovieId == movie.Id) ?
                    _context.MovieRatings.Where(r => r.MovieId == movie.Id).Average(o => o.Rating) : 0
            };
        }

        public async Task<IEnumerable<MovieDomainModel>> GetAsync(MovieFilterCriteria criteria)
        {
            if (criteria == null)
                throw new ArgumentNullException(nameof(criteria));

            var movieQuery = _context.Movies.AsQueryable();

            if (!string.IsNullOrWhiteSpace(criteria.Title))
                movieQuery = movieQuery.Where(movie => movie.Title.ToLower().Contains(criteria.Title.ToLower()));

            if (criteria.YearOfRelease != null)
                movieQuery = movieQuery.Where(movie => movie.YearOfRelease == criteria.YearOfRelease.Value);

            if (!string.IsNullOrWhiteSpace(criteria.Genres))
                movieQuery = movieQuery.Where(movie => movie.Genres.ToLower().Contains(criteria.Genres.ToLower()));

            var filteredMovies = await movieQuery.ToListAsync();

            return filteredMovies.Select(movie => new MovieDomainModel
            {
                Id = movie.Id,
                Title = movie.Title,
                YearOfRelease = movie.YearOfRelease,
                RunningTime = movie.RunningTime,
                AverageRating = _context.MovieRatings.Any(r => r.MovieId == movie.Id) ? 
                    _context.MovieRatings.Where(r => r.MovieId == movie.Id).Average(o => o.Rating) : 0
            });
        }

        public async Task<IEnumerable<MovieDomainModel>> GetTopRanked(Guid? userId)
        {
            var dataToReturn = new List<MovieDomainModel>();
            var query = _context.MovieRatings.AsQueryable();

            if (userId.HasValue)
                query = _context.MovieRatings.Where(o => o.UserId == userId);

            var topRatedMovies = await query
                .GroupBy(o => o.MovieId)
                .Select(p => new
                {
                    Id = p.Key,
                    AverageRating = p.Average(o => o.Rating)
                })
                .OrderBy(o => o.AverageRating)
                .Take(5)
                .ToListAsync();

            foreach (var movie in topRatedMovies)
            {
                var additionalInfo = _context.Movies.Single(m => m.Id == movie.Id);

                dataToReturn.Add(new MovieDomainModel
                {
                    Id = movie.Id,
                    Title = additionalInfo.Title,
                    YearOfRelease = additionalInfo.YearOfRelease,
                    RunningTime = additionalInfo.RunningTime,
                    AverageRating = movie.AverageRating
                });
            }

            return dataToReturn;
        }
    }
}
