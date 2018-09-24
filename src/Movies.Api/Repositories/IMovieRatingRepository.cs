using System;
using System.Threading.Tasks;

namespace Movies.Api.Repositories
{
    public interface IMovieRatingRepository
    {
        Task AddAsync(Guid userId, Guid movieId, double rating);
    }
}
