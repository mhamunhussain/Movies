namespace Movies.Api.Models
{
    public class MovieFilterCriteria
    {
        public string Title { get; set; }
        public int? YearOfRelease { get; set; }
        public string Genres { get; set; }
    }
}
