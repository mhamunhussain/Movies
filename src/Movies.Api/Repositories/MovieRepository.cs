using Movies.Api.Data;
using Movies.Api.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Movies.Api.Repositories
{
    public class MovieRepository : IMovieRepository
    {
        private ApplicationContext _context;

        public MovieRepository(ApplicationContext context)
        {
            _context = context;
        }

        public Task<IEnumerable<MovieDomainModel>> GetAsync(MovieFilterCriteria criteria)
        {
            var x = new List<MovieDomainModel>
            {
                new MovieDomainModel { Id = Guid.NewGuid() },
                new MovieDomainModel { Id = Guid.NewGuid() },
            };

            return Task.FromResult<IEnumerable<MovieDomainModel>>(x);
        }
    }
}
