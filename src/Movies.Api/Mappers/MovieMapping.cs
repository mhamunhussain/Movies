using AutoMapper;
using Movies.Api.Models;

namespace Movies.Api.Mappers
{
    public class MovieMapping : Profile
    {
        public MovieMapping()
        {
            CreateMap<MovieDomainModel, MovieViewModel>()
                .ReverseMap();
        }
    }
}
