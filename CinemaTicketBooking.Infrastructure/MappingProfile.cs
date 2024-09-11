using AutoMapper;
using Microsoft.IdentityModel.Tokens;

namespace CinemaTicketBooking.Infrastructure;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Create mappings between infrastructure and domain entities
        CreateMap<Infrastructure.Entities.Movie, Domain.Entities.Movie>()
            .ForMember(dest => dest.Duration, opt => opt.MapFrom(src => TimeSpan.FromSeconds(src.DurationInSeconds)))
            .ForMember(dest => dest.Genre, opt => opt.MapFrom(src => ConvertGenreCollectionToGenreFlags(src.Genres)));

        // Additional mappings can be added here
    }

    private Domain.Genre ConvertGenreCollectionToGenreFlags(ICollection<Entities.Genre> genreCollection)
    {
        if (genreCollection.IsNullOrEmpty())
            return Domain.Genre.Unknown;

        var parsedGenre = new Domain.Genre();
        foreach (var genreEntry in genreCollection)
        {
            if (Enum.TryParse<Domain.Genre>(genreEntry.Name, ignoreCase: true, out var parsedGenreEntry))
            {
                parsedGenre |= parsedGenreEntry;
            }
            else
            {
                // TODO Log here a warning or throw an exception if this shall fail,
                // otherwise we could just note that some of the genres are not known to us
                parsedGenre |= Domain.Genre.Unknown;
            }
        }

        return parsedGenre;
    }
}
