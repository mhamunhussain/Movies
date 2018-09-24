using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Movies.Api.Models;
using Movies.Api.Repositories;
using Movies.Api.Validators;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Movies.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/v1/[controller]")]
    public class MoviesController : Controller
    {
        private readonly IMovieRepository _movieRepository;
        private readonly IMovieRequestValidator _movieRequestValidator;
        private readonly IMapper _mapper;

        public MoviesController(
            IMovieRepository movieRepository,
            IMovieRequestValidator movieRequestValidator,
            IMapper mapper)
        {
            _movieRepository = movieRepository;
            _movieRequestValidator = movieRequestValidator;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Get(
            [FromQuery] string title, 
            [FromQuery] int? yearOfRelease, 
            [FromQuery] string genre)
        {
            var validRequest = _movieRequestValidator.Validate(title, yearOfRelease, genre);

            if (!validRequest)
                return BadRequest();

            var movies = await _movieRepository.GetAsync(new MovieFilterCriteria
            {
                Title = title, 
                YearOfRelease = yearOfRelease,
                Genres = genre
            });

            if (!movies.Any())
                return NotFound();

            return Ok(_mapper.Map<IEnumerable<MovieViewModel>>(
                movies.OrderBy(m => m.AverageRating)
                      .ThenBy(m => m.Title)));
        }

        [Route("top-ranked")]
        [HttpGet]
        public async Task<IActionResult> GetTopRanked()
        {
            var movies = await _movieRepository.GetTopRanked(null);

            return Ok(_mapper.Map<IEnumerable<MovieViewModel>>(
                movies.OrderByDescending(o => o.AverageRating)
                      .ThenBy(o => o.Title)));
        }
    }
}
