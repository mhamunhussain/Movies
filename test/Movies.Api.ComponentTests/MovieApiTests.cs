using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Movies.Api.ComponentTests.Helpers;
using Movies.Api.Data;
using Movies.Api.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Movies.Api.ComponentTests
{
    public class MovieApiTests : IClassFixture<TestServerFixture>
    {
        private readonly TestServerFixture _fixture;
        private readonly ApplicationDbContext _dbContext;

        public MovieApiTests(TestServerFixture fixture)
        {
            _fixture = fixture;
            _dbContext = _fixture.DbContext;
        }

        [Fact]
        public async Task GivenGetMovieIsRequestedWhenMovieExistsThenReturnMovie()
        {
            var rating = 3.249;
            var roundRating = 3.0;

            var testMovie = new Movie
            {
                Id = Guid.NewGuid(),
                Title = "Test Movie",
                Genres = "comedy",
                YearOfRelease = 2018,
                RunningTime = 90
            };

            var alternativeMovie = new Movie
            {
                Id = Guid.NewGuid(),
                Title = "Alternative Movie",
                Genres = "comedy",
                YearOfRelease = 2018,
                RunningTime = 90
            };

            var movieRating = new MovieRating
            {
                Id = Guid.NewGuid(),
                MovieId = testMovie.Id,
                Rating = rating,
                UserId = Guid.NewGuid()
            };

            await CreateTestMovies(new[] { testMovie, alternativeMovie });
            await CreateTestMovieRating(new[] {movieRating});

            var response = await _fixture.Client.GetAsync(
                $"api/v1/movies?title={testMovie.Title}&yearOfRelease={testMovie.YearOfRelease}&genre={testMovie.Genres}");

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var content = await response.Content.ReadAsStringAsync();
            var results = JsonConvert.DeserializeObject<IEnumerable<MovieViewModel>>(content);

            results.Count().Should().Be(1);

            var movieResult = results.First();
            movieResult.Id.Should().Be(testMovie.Id);
            movieResult.AverageRating.Should().Be(roundRating);
        }

        [Fact]
        public async Task GivenGetTopRankedMovieIsRequestedThenReturnTopRankedInOrder()
        {
            var testMovie = new Movie
            {
                Id = Guid.NewGuid(),
                Title = "A Movie",
                Genres = "comedy",
                YearOfRelease = 2018,
                RunningTime = 90
            };

            var testMovie2 = new Movie
            {
                Id = Guid.NewGuid(),
                Title = "B Movie",
                Genres = "comedy",
                YearOfRelease = 2018,
                RunningTime = 90
            };

            var movieRating = new MovieRating
            {
                Id = Guid.NewGuid(),
                MovieId = testMovie.Id,
                Rating = 10,
                UserId = Guid.NewGuid()
            };

            var movieRating2 = new MovieRating
            {
                Id = Guid.NewGuid(),
                MovieId = testMovie2.Id,
                Rating = 10,
                UserId = Guid.NewGuid()
            };

            await CreateTestMovies(new[] { testMovie, testMovie2 });
            await CreateTestMovieRating(new[] { movieRating, movieRating2 });

            var response = await _fixture.Client.GetAsync("api/v1/movies/top-ranked");

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var content = await response.Content.ReadAsStringAsync();
            var results = JsonConvert.DeserializeObject<IEnumerable<MovieViewModel>>(content);

            results.Count().Should().Be(2);

            var firstResult = results.ElementAt(0);
            var secondResult = results.ElementAt(1);
            firstResult.Id.Should().Be(testMovie.Id);
            secondResult.Id.Should().Be(testMovie2.Id);
        }

        [Fact]
        public async Task GivenGetTopRankedMoviesByUserThenReturnTopRanked()
        {
            var testMovie = new Movie
            {
                Id = Guid.NewGuid(),
                Title = "A Movie",
                Genres = "comedy",
                YearOfRelease = 2018,
                RunningTime = 90
            };

            var testMovie2 = new Movie
            {
                Id = Guid.NewGuid(),
                Title = "B Movie",
                Genres = "comedy",
                YearOfRelease = 2018,
                RunningTime = 90
            };

            var user = new User
            {
                Id = Guid.NewGuid(),
                FullName = "Test User"
            };

            var movieRating = new MovieRating
            {
                Id = Guid.NewGuid(),
                MovieId = testMovie.Id,
                Rating = 10,
                UserId = user.Id
            };

            await CreateTestMovies(new[] { testMovie, testMovie2 });
            await CreateTestUser(user);
            await CreateTestMovieRating(new[] { movieRating });

            var response = await _fixture.Client.GetAsync($"api/v1/users/{user.Id}/movies/top-ranked");

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var content = await response.Content.ReadAsStringAsync();
            var results = JsonConvert.DeserializeObject<IEnumerable<MovieViewModel>>(content);

            results.Count().Should().Be(1);
            results.First().Id.Should().Be(testMovie.Id);
        }

        [Fact]
        public async Task GivenAddRatingIsInvokedThenAddRating()
        {
            var rating = 5; 

            var testMovie = new Movie
            {
                Id = Guid.NewGuid(),
                Title = "A Movie",
                Genres = "comedy",
                YearOfRelease = 2018,
                RunningTime = 90
            };

            var user = new User
            {
                Id = Guid.NewGuid(),
                FullName = "Test User"
            };

            await CreateTestMovies(new[] { testMovie });
            await CreateTestUser(user);

            var json = JsonConvert.SerializeObject(rating, Formatting.Indented);
            var requestBody = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _fixture.Client.PostAsync($"api/v1/users/{user.Id}/movies/{testMovie.Id}/rating", requestBody);

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var movieRating = await _dbContext.MovieRatings.Where(o => o.MovieId == testMovie.Id).SingleAsync();
            movieRating.MovieId.Should().Be(testMovie.Id);
            movieRating.UserId.Should().Be(user.Id);
            movieRating.Rating.Should().Be(rating);
        }

        private async Task CreateTestMovies(IEnumerable<Movie> movies)
        {
            await _dbContext.Movies.AddRangeAsync(movies);
            await _dbContext.SaveChangesAsync();
        }

        private async Task CreateTestUser(User user)
        {
            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();
        }

        private async Task CreateTestMovieRating(IEnumerable<MovieRating> ratings)
        {
            await _dbContext.MovieRatings.AddRangeAsync(ratings);
            await _dbContext.SaveChangesAsync();
        }
    }
}