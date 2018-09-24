using System.Linq;

namespace Movies.Api.Validators
{
    public class MovieRequestValidator : IMovieRequestValidator
    {
        public bool Validate(string title, int? yearOfRelease, string genres)
        {
            if (string.IsNullOrWhiteSpace(title) &&
                yearOfRelease == null &&
                string.IsNullOrWhiteSpace(genres))
            {
                return false;
            }

            return true;
        }
    }
}
