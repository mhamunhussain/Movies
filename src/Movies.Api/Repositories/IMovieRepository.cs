using Movies.Api.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Movies.Api.Repositories
{
    public interface IMovieRepository
    {
        Task<IEnumerable<MovieDomainModel>> GetAsync(MovieFilterCriteria criteria);
    }
}
