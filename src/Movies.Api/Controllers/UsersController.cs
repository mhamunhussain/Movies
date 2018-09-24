using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Movies.Api.Models;
using Movies.Api.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Movies.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/v1/[controller]")]
    public class UsersController : Controller
    {
        private readonly IMovieRepository _movieRepository;
        private readonly IMovieRatingRepository _movieRatingRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UsersController(
            IMovieRepository movieRepository,
            IMovieRatingRepository movieRatingRepository,
            IUserRepository userRepository,
            IMapper mapper)
        {
            _movieRepository = movieRepository;
            _movieRatingRepository = movieRatingRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        [Route("{userId}/movies/top-ranked")]
        [HttpGet]
        public async Task<IActionResult> GetTopRanked(Guid userId)
        {
            var user = await _userRepository.GetAsync(userId);
            var movies = await _movieRepository.GetTopRanked(userId);

            if (user == null || !movies.Any())
                return NotFound();

            return Ok(_mapper.Map<IEnumerable<MovieViewModel>>(movies));
        }

        [Route("{userId}/movies/{movieId}/rating")]
        [HttpPost]
        public async Task<IActionResult> AddRating(Guid userId, Guid movieId, [FromBody] int rating)
        {
            if (rating == 0 || rating > 5)
                return BadRequest("Rating must be between 1 and 5");

            var user = await _userRepository.GetAsync(userId);
            var movie = await _movieRepository.GetAsync(movieId);

            if (user == null || movie == null)
                return NotFound();

            await _movieRatingRepository.AddAsync(userId, movieId, rating);

            return Ok();
        }
    }
}
