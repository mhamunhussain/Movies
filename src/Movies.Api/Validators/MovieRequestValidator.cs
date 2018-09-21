using FluentValidation;
using Movies.Api.Models;

namespace Movies.Api.Validators
{
    public class MovieRequestValidator : AbstractValidator<MovieFilterCriteria>
    {
        public MovieRequestValidator()
        {
            RuleFor(request => request)
                .Must(request => 
                    !string.IsNullOrWhiteSpace(request.Title) ||
                    !string.IsNullOrWhiteSpace(request.Genre) ||
                    IsValidYear(request.YearOfRelease))
                .WithMessage("request must contain valid filters");
        }

        private static bool IsValidYear(string yearOfRelease) =>
            int.TryParse(yearOfRelease, out var _);
    }
}
