using FluentAssertions;
using Movies.Api.Validators;
using Xunit;

namespace Movies.Api.UnitTests.Validators
{
    public class MovieRequestValidatorTests
    {
        private readonly MovieRequestValidator _validator;

        public MovieRequestValidatorTests()
        {
            _validator = new MovieRequestValidator();
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void GivenValidateIsInvokedWhenFiltersAreAllEmptyThenFailValidation(string value)
        {
            var result = _validator.Validate(value, null, value);

            result.Should().BeFalse();
        }

        [Theory]
        [InlineData("", 2000, "genre")]
        [InlineData("title", null, "genre")]
        [InlineData("title", 2000, "")]
        [InlineData("title", 2000, "genre")]
        public void GivenValidateIsInvokedWhenFiltersContainValueThenPassValidation(string title, int? year, string genre)
        {
            var result = _validator.Validate(title, year, genre);

            result.Should().BeTrue();
        }
    }
}
