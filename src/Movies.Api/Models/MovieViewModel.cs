using System;

namespace Movies.Api.Models
{
    public class MovieViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public int YearOfRelease { get; set; }
        public int RunningTime { get; set; }
        public double AverageRating { get; set; }
    }
}
