using AutoMapper;
using CinemaTicketBooking.Domain;
using CinemaTicketBooking.Domain.Entities;
using CinemaTicketBooking.Application.Dtos;

namespace CinemaTicketBooking.Application;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Create mappings between domain models and DTOs
        CreateMap<Movie, MovieDto>()
            .ForMember(dest => dest.Genres, opt => opt.MapFrom(src => ConvertMovieGenreToListOfStrings(src.Genre)))
            .ForMember(dest => dest.DurationInSeconds, opt => opt.MapFrom(src => (int)src.Duration.TotalSeconds));

        // Additional mappings can be added here
    }

    private List<string> ConvertMovieGenreToListOfStrings(Genre genreFlags)
    {
        var genres = Enum.GetValues(typeof(Genre))
                         .Cast<Genre>()
                         .Where(flag => genreFlags.HasFlag(flag) && flag != Genre.Unknown)
                         .Select(flag => flag.ToString())
                         .ToList();

        return genres;
    }
}
