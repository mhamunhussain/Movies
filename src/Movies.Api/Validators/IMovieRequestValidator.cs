namespace Movies.Api.Validators
{
    public interface IMovieRequestValidator
    {
        bool Validate(string title, int? yearOfRelease, string genres);
    }
}
