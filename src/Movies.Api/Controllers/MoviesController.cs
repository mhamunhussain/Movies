using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Movies.Api.Models;
using Movies.Api.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Movies.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/v1/[controller]")]
    public class MoviesController : Controller
    {
        private readonly IMovieRepository _movieRepository;
        private readonly IValidator<MovieFilterCriteria> _movieRequestValidator;
        private readonly IMapper _mapper;

        public MoviesController(
            IMovieRepository movieRepository, 
            IValidator<MovieFilterCriteria> movieRequestValidator,
            IMapper mapper)
        {
            _movieRepository = movieRepository;
            _movieRequestValidator = movieRequestValidator;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string title, [FromQuery] string yearOfRelease, [FromQuery] string genre)
        {
            var criteria = new MovieFilterCriteria
            {
                Title = title,
                Genre = genre,
                YearOfRelease = yearOfRelease
            };

            var validationResult = _movieRequestValidator.Validate(criteria);

            if (!validationResult.IsValid)
                return BadRequest();

            var movies = await _movieRepository.GetAsync(criteria);

            return Ok(_mapper.Map<IEnumerable<MovieViewModel>>(movies));
        }

        [Route("top-ranked")]
        [HttpGet]
        public Task<IActionResult> GetTopRanked()
        {
            throw new NotImplementedException();
        }
    }
}
