using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Movies.Api.Controllers;
using Movies.Api.Models;
using Movies.Api.Repositories;
using Movies.Api.Validators;
using System.Threading.Tasks;
using Xunit;

namespace Movies.Api.UnitTests.Controllers
{
    public class MoviesControllerTests
    {
        private readonly Mock<IMovieRepository> _movieRepository;
        private readonly Mock<IMovieRequestValidator> _movieRequestValidator;
        private readonly MoviesController _controller;

        public MoviesControllerTests()
        {
            _movieRepository = new Mock<IMovieRepository>();
            _movieRequestValidator = new Mock<IMovieRequestValidator>();
            _controller = new MoviesController(
                _movieRepository.Object, 
                _movieRequestValidator.Object, 
                new Mock<IMapper>().Object);

            _movieRequestValidator.Setup(validator => validator.Validate(
                It.IsAny<string>(),
                It.IsAny<int>(),
                It.IsAny<string>())).Returns(true);
        }

        [Fact(DisplayName = "Given get is invoked when request is not valid then return bad request")]
        public async Task GivenGetIsInvokedWhenValidationFailsThenReturnBadRequest()
        {
            _movieRequestValidator.Setup(validator => validator.Validate(
                It.IsAny<string>(),
                It.IsAny<int>(),
                It.IsAny<string>())).Returns(false);

            var result = await _controller.Get("title", 2000, "genre");

            result.Should().BeOfType<BadRequestResult>();
            _movieRepository.Verify(repo => repo.GetAsync(It.IsAny<MovieFilterCriteria>()), Times.Never);
        }

        [Fact(DisplayName = "Given get is invoked when movies do not exists then return not found")]
        public async Task GivenGetIsInvokedWhenMoviesDoNotExistThenReturnNotFound()
        {
            var result = await _controller.Get("title", 2000, "genre");

            result.Should().BeOfType<NotFoundResult>();
            _movieRepository.Verify(repo => repo.GetAsync(It.IsAny<MovieFilterCriteria>()), Times.Once);
        }

        [Fact(DisplayName = "Given get is invoked when movies exist then return data")]
        public async Task GivenGetIsInvokedWhenMoviesExistThenReturnData()
        {
            _movieRepository.Setup(repo => repo.GetAsync(It.IsAny<MovieFilterCriteria>()))
                .ReturnsAsync(new[] {new MovieDomainModel()});

            var result = await _controller.Get("title", 2000, "genre");

            result.Should().BeOfType<OkObjectResult>();
            _movieRepository.Verify(repo => repo.GetAsync(It.IsAny<MovieFilterCriteria>()), Times.Once);
        }
    }
}
