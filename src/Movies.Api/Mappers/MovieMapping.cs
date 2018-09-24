using AutoMapper;
using Movies.Api.Models;
using System;

namespace Movies.Api.Mappers
{
    public class MovieMapping : Profile
    {
        public MovieMapping()
        {
            CreateMap<MovieDomainModel, MovieViewModel>()
                .ForMember(dest => dest.AverageRating, 
                           opts => opts.MapFrom(
                               src => ApplyRounding(src.AverageRating)))
                .ReverseMap();
        }

        private static double ApplyRounding(double value) =>
             Math.Round(value * 2) / 2;
    }
}
