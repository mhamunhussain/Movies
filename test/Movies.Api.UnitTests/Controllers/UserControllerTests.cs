using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Movies.Api.Controllers;
using Movies.Api.Models;
using Movies.Api.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Movies.Api.UnitTests.Controllers
{
    public class UserControllerTests
    {
        private readonly Mock<IMovieRepository> _movieRepository;
        private readonly Mock<IMovieRatingRepository> _movieRatingRepository;
        private readonly Mock<IUserRepository> _userRepository;
        private readonly UsersController _controller;

        public UserControllerTests()
        {
            _movieRepository = new Mock<IMovieRepository>();
            _movieRatingRepository = new Mock<IMovieRatingRepository>();
            _userRepository = new Mock<IUserRepository>();
            _controller = new UsersController(
                _movieRepository.Object,
                _movieRatingRepository.Object,
                _userRepository.Object,
                new Mock<IMapper>().Object);
        }

        [Fact]
        public async Task GivenGetTopRankedIsInvokedWhenUserDoesNotExistThenReturnNotFound()
        {
            var result = await _controller.GetTopRanked(Guid.NewGuid());

            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task GivenGetTopRankedIsInvokedWhenMovieDoesNotExistThenReturnNotFound()
        {
            _userRepository.Setup(repo => repo.GetAsync(It.IsAny<Guid>()))
                .ReturnsAsync(new UserDomainModel());

            var result = await _controller.GetTopRanked(Guid.NewGuid());

            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task GivenGetTopRankedIsInvokedWhenDataExistsThenReturnMovies()
        {
            _userRepository.Setup(repo => repo.GetAsync(It.IsAny<Guid>()))
                .ReturnsAsync(new UserDomainModel());

            _movieRepository.Setup(repo => repo.GetTopRanked(It.IsAny<Guid>()))
                .ReturnsAsync(new List<MovieDomainModel>
                {
                    new MovieDomainModel()
                });

            var result = await _controller.GetTopRanked(Guid.NewGuid());

            result.Should().BeOfType<OkObjectResult>();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(6)]
        public async Task GivenAddRatingIsInvokedWhenRatingIsInvalidThenReturnBadRequest(int value)
        {
            var result = await _controller.AddRating(Guid.NewGuid(), Guid.NewGuid(), value);

            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task GivenAddRatingIsInvokedWhenUserDoesNotExistThenReturnNotFound()
        {
            _movieRepository.Setup(repo => repo.GetAsync(It.IsAny<Guid>()))
                .ReturnsAsync(new MovieDomainModel());

            var result = await _controller.AddRating(Guid.NewGuid(), Guid.NewGuid(), 3);

            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task GivenAddRatingIsInvokedWhenMovieDoesNotExistThenReturnNotFound()
        {
            _userRepository.Setup(repo => repo.GetAsync(It.IsAny<Guid>()))
                .ReturnsAsync(new UserDomainModel());

            var result = await _controller.AddRating(Guid.NewGuid(), Guid.NewGuid(), 3);

            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task GivenAddRatingIsInvokedWhenDataExistsThenAddRating()
        {
            var movieId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var rating = 3;

            _movieRepository.Setup(repo => repo.GetAsync(movieId))
                .ReturnsAsync(new MovieDomainModel());

            _userRepository.Setup(repo => repo.GetAsync(userId))
                .ReturnsAsync(new UserDomainModel());

            var result = await _controller.AddRating(userId, movieId, rating);

            result.Should().BeOfType<OkResult>();

            _movieRatingRepository.Verify(o => o.AddAsync(userId, movieId, rating), Times.Once());
        }
    }
}
