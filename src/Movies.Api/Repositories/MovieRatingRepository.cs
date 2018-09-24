using Microsoft.EntityFrameworkCore;
using Movies.Api.Data;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Movies.Api.Repositories
{
    public class MovieRatingRepository : IMovieRatingRepository
    {
        private readonly ApplicationDbContext _context;

        public MovieRatingRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Guid userId, Guid movieId, double rating)
        {
            var existingRating = await _context.MovieRatings
                .Where(o => o.UserId == userId && o.MovieId == movieId)
                .SingleOrDefaultAsync();

            if (existingRating != null)
            {
                existingRating.Rating = rating;
                _context.MovieRatings.Update(existingRating);
            }
            else
            {
                await _context.MovieRatings.AddAsync(new MovieRating
                {
                    Id = Guid.NewGuid(),
                    MovieId = movieId,
                    UserId = userId,
                    Rating = rating
                });
            }

            await _context.SaveChangesAsync();
        }
    }
}
