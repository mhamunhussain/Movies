using Movies.Api.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Movies.Api.Repositories
{
    public interface IMovieRepository
    {
        Task<MovieDomainModel> GetAsync(Guid movieId);
        Task<IEnumerable<MovieDomainModel>> GetAsync(MovieFilterCriteria criteria);
        Task<IEnumerable<MovieDomainModel>> GetTopRanked(Guid? userId);
    }
}
