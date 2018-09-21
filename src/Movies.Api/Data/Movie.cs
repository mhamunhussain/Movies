using System;

namespace Movies.Api.Data
{
    public class Movie
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public int YearOfRelease { get; set; }
        public int Rating { get; set; }
        public int RunningTime { get; set; }
    }
}
