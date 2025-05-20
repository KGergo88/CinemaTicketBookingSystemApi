using CinemaTicketBooking.Infrastructure.DatabaseSeeding;
using CinemaTicketBooking.Infrastructure.Entities;

namespace CinemaTicketBooking.IntegrationTests;

internal class DefaultSeedData : SeedData
{
    public DefaultSeedData()
    {
        var darkFantasyGenre = new GenreEntity{ Name = "Dark Fantasy", Movies = [] };
        var slasherHorrorGenre = new GenreEntity{ Name = "Slasher Horror", Movies = [] };
        var supernaturalHorrorGenre = new GenreEntity{ Name = "Supernatural Horror", Movies = [] };
        var fantasyGenre = new GenreEntity{ Name = "Fantasy", Movies = [] };
        var horrorGenre = new GenreEntity{ Name = "Horror", Movies = [] };
        var misteryGenre = new GenreEntity{ Name = "Mistery", Movies = [] };
        var dystopianSciFiGenre = new GenreEntity{ Name = "Dystopian Sci-Fi", Movies = [] };
        var survivalGenre = new GenreEntity{ Name = "Survival", Movies = [] };
        var zombieHorrorGenre = new GenreEntity{ Name = "Zombie Horror", Movies = [] };
        var actionGenre = new GenreEntity{ Name = "Action", Movies = [] };
        var dramaGenre = new GenreEntity{ Name = "Drama", Movies = [] };
        var sciFiGenre = new GenreEntity{ Name = "Sci-Fi", Movies = [] };
        var thrillerGenre = new GenreEntity{ Name = "Thriller", Movies = [] };
        var psychologicalDramaGenre = new GenreEntity{ Name = "Psychological Drama", Movies = [] };
        var tragedyGenre = new GenreEntity{ Name = "Tragedy", Movies = [] };

        var sleepyHollowMovie = new MovieEntity
        {
            Id = Guid.NewGuid(),
            Title = "Sleepy Hollow",
            ReleaseYear = 1999,
            Description = "A movie about a headless horseman chopping other people´s heads off",
            DurationInSeconds = 105,
            Genres = [
                darkFantasyGenre,
                slasherHorrorGenre,
                supernaturalHorrorGenre,
                fantasyGenre,
                horrorGenre,
                misteryGenre
            ]
        };

        var iAmLegendMovie = new MovieEntity
        {
            Id = Guid.NewGuid(),
            Title = "I Am Legend",
            ReleaseYear = 2007,
            Description = "A movie about people turning into zombies after getting vaccinated",
            DurationInSeconds = 101,
            Genres = [
                dystopianSciFiGenre,
                survivalGenre,
                zombieHorrorGenre,
                actionGenre,
                dramaGenre,
                horrorGenre,
                sciFiGenre,
                thrillerGenre
            ]
        };

        var melancholiaMovie = new MovieEntity
        {
            Id = Guid.NewGuid(),
            Title = "Melancholia",
            ReleaseYear = 2011,
            Description = "Two sisters find their already strained relationship challenged as a mysterious new planet threatens to collide with Earth.",
            DurationInSeconds = 8100,
            Genres = [
                psychologicalDramaGenre,
                tragedyGenre,
                dramaGenre,
                sciFiGenre
            ]
        };

        var elitMoziTheater = new TheaterEntity
        {
            Id = Guid.NewGuid(),
            Name = "Elit Mozi",
            Address = "9400 Sopron, Torna utca 14",
        };

        var elitMoziHuszarikTeremAuditorium = new AuditoriumEntity
        {
            Id = Guid.NewGuid(),
            Name = "Huszárik terem",
            TheaterId = elitMoziTheater.Id
        };

        var elitMoziHuszarikTeremNezoterTier = new TierEntity
        {
            Id = Guid.NewGuid(),
            Name = "Nézötér",
            AuditoriumId = elitMoziHuszarikTeremAuditorium.Id
        };

        var elitMoziHuszarikTeremNezoterSeats = new List<SeatEntity>()
        {
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziHuszarikTeremNezoterTier.Id, Row = 1, Column = 1 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziHuszarikTeremNezoterTier.Id, Row = 1, Column = 2 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziHuszarikTeremNezoterTier.Id, Row = 1, Column = 3 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziHuszarikTeremNezoterTier.Id, Row = 1, Column = 4 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziHuszarikTeremNezoterTier.Id, Row = 2, Column = 1 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziHuszarikTeremNezoterTier.Id, Row = 2, Column = 2 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziHuszarikTeremNezoterTier.Id, Row = 2, Column = 3 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziHuszarikTeremNezoterTier.Id, Row = 2, Column = 4 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziHuszarikTeremNezoterTier.Id, Row = 3, Column = 1 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziHuszarikTeremNezoterTier.Id, Row = 3, Column = 2 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziHuszarikTeremNezoterTier.Id, Row = 3, Column = 3 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziHuszarikTeremNezoterTier.Id, Row = 3, Column = 4 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziHuszarikTeremNezoterTier.Id, Row = 4, Column = 1 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziHuszarikTeremNezoterTier.Id, Row = 4, Column = 2 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziHuszarikTeremNezoterTier.Id, Row = 4, Column = 3 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziHuszarikTeremNezoterTier.Id, Row = 4, Column = 4 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziHuszarikTeremNezoterTier.Id, Row = 5, Column = 1 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziHuszarikTeremNezoterTier.Id, Row = 5, Column = 2 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziHuszarikTeremNezoterTier.Id, Row = 5, Column = 3 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziHuszarikTeremNezoterTier.Id, Row = 5, Column = 4 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziHuszarikTeremNezoterTier.Id, Row = 6, Column = 1 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziHuszarikTeremNezoterTier.Id, Row = 6, Column = 2 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziHuszarikTeremNezoterTier.Id, Row = 6, Column = 3 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziHuszarikTeremNezoterTier.Id, Row = 6, Column = 4 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziHuszarikTeremNezoterTier.Id, Row = 7, Column = 1 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziHuszarikTeremNezoterTier.Id, Row = 7, Column = 2 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziHuszarikTeremNezoterTier.Id, Row = 7, Column = 3 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziHuszarikTeremNezoterTier.Id, Row = 7, Column = 4 }
        };

        var elitMoziNagyteremAuditorium = new AuditoriumEntity
        {
            Id = Guid.NewGuid(),
            Name = "Nagyterem",
            TheaterId = elitMoziTheater.Id
        };

        var elitMoziNagyteremErkelyTier = new TierEntity
        {
            Id = Guid.NewGuid(),
            Name = "Erkély",
            AuditoriumId = elitMoziNagyteremAuditorium.Id,
        };

        var elitMoziNagyteremNezoterTier = new TierEntity
        {
            Id = Guid.NewGuid(),
            Name = "Nézőtér",
            AuditoriumId = elitMoziNagyteremAuditorium.Id,
        };

        var elitMoziNagyteremErkelySeats = new List<SeatEntity>()
        {
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremErkelyTier.Id, Row = 1, Column = 1 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremErkelyTier.Id, Row = 1, Column = 2 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremErkelyTier.Id, Row = 1, Column = 3 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremErkelyTier.Id, Row = 1, Column = 4 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremErkelyTier.Id, Row = 1, Column = 5 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremErkelyTier.Id, Row = 1, Column = 6 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremErkelyTier.Id, Row = 1, Column = 7 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremErkelyTier.Id, Row = 1, Column = 8 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremErkelyTier.Id, Row = 1, Column = 9 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremErkelyTier.Id, Row = 1, Column = 10 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremErkelyTier.Id, Row = 1, Column = 11 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremErkelyTier.Id, Row = 1, Column = 12 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremErkelyTier.Id, Row = 1, Column = 13 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremErkelyTier.Id, Row = 1, Column = 14 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremErkelyTier.Id, Row = 1, Column = 15 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremErkelyTier.Id, Row = 1, Column = 16 },

            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremErkelyTier.Id, Row = 2, Column = 1 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremErkelyTier.Id, Row = 2, Column = 2 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremErkelyTier.Id, Row = 2, Column = 3 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremErkelyTier.Id, Row = 2, Column = 4 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremErkelyTier.Id, Row = 2, Column = 5 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremErkelyTier.Id, Row = 2, Column = 6 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremErkelyTier.Id, Row = 2, Column = 7 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremErkelyTier.Id, Row = 2, Column = 8 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremErkelyTier.Id, Row = 2, Column = 9 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremErkelyTier.Id, Row = 2, Column = 10 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremErkelyTier.Id, Row = 2, Column = 11 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremErkelyTier.Id, Row = 2, Column = 12 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremErkelyTier.Id, Row = 2, Column = 13 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremErkelyTier.Id, Row = 2, Column = 14 },

            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremErkelyTier.Id, Row = 3, Column = 1 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremErkelyTier.Id, Row = 3, Column = 2 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremErkelyTier.Id, Row = 3, Column = 3 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremErkelyTier.Id, Row = 3, Column = 4 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremErkelyTier.Id, Row = 3, Column = 5 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremErkelyTier.Id, Row = 3, Column = 6 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremErkelyTier.Id, Row = 3, Column = 7 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremErkelyTier.Id, Row = 3, Column = 8 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremErkelyTier.Id, Row = 3, Column = 9 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremErkelyTier.Id, Row = 3, Column = 10 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremErkelyTier.Id, Row = 3, Column = 11 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremErkelyTier.Id, Row = 3, Column = 12 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremErkelyTier.Id, Row = 3, Column = 13 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremErkelyTier.Id, Row = 3, Column = 14 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremErkelyTier.Id, Row = 3, Column = 15 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremErkelyTier.Id, Row = 3, Column = 16 },

            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremErkelyTier.Id, Row = 4, Column = 1 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremErkelyTier.Id, Row = 4, Column = 2 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremErkelyTier.Id, Row = 4, Column = 3 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremErkelyTier.Id, Row = 4, Column = 4 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremErkelyTier.Id, Row = 4, Column = 5 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremErkelyTier.Id, Row = 4, Column = 6 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremErkelyTier.Id, Row = 4, Column = 7 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremErkelyTier.Id, Row = 4, Column = 8 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremErkelyTier.Id, Row = 4, Column = 9 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremErkelyTier.Id, Row = 4, Column = 10 }
        };

        var elitMoziNagyteremNezoterSeats = new List<SeatEntity>()
        {
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 1, Column = 1 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 1, Column = 2 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 1, Column = 3 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 1, Column = 4 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 1, Column = 5 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 1, Column = 6 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 1, Column = 7 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 1, Column = 8 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 1, Column = 9 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 1, Column = 10 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 1, Column = 11 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 1, Column = 12 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 1, Column = 13 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 1, Column = 14 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 1, Column = 15 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 1, Column = 16 },

            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 2, Column = 1 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 2, Column = 2 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 2, Column = 3 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 2, Column = 4 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 2, Column = 5 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 2, Column = 6 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 2, Column = 7 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 2, Column = 8 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 2, Column = 9 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 2, Column = 10 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 2, Column = 11 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 2, Column = 12 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 2, Column = 13 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 2, Column = 14 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 2, Column = 15 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 2, Column = 16 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 2, Column = 17 },

            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 3, Column = 1 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 3, Column = 2 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 3, Column = 3 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 3, Column = 4 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 3, Column = 5 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 3, Column = 6 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 3, Column = 7 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 3, Column = 8 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 3, Column = 9 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 3, Column = 10 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 3, Column = 11 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 3, Column = 12 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 3, Column = 13 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 3, Column = 14 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 3, Column = 15 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 3, Column = 16 },

            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 4, Column = 1 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 4, Column = 2 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 4, Column = 3 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 4, Column = 4 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 4, Column = 5 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 4, Column = 6 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 4, Column = 7 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 4, Column = 8 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 4, Column = 9 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 4, Column = 10 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 4, Column = 11 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 4, Column = 12 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 4, Column = 13 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 4, Column = 14 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 4, Column = 15 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 4, Column = 16 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 4, Column = 17 },

            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 5, Column = 1 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 5, Column = 2 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 5, Column = 3 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 5, Column = 4 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 5, Column = 5 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 5, Column = 6 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 5, Column = 7 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 5, Column = 8 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 5, Column = 9 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 5, Column = 10 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 5, Column = 11 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 5, Column = 12 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 5, Column = 13 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 5, Column = 14 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 5, Column = 15 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 5, Column = 16 },

            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 6, Column = 1 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 6, Column = 2 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 6, Column = 3 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 6, Column = 4 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 6, Column = 5 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 6, Column = 6 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 6, Column = 7 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 6, Column = 8 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 6, Column = 9 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 6, Column = 10 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 6, Column = 11 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 6, Column = 12 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 6, Column = 13 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 6, Column = 14 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 6, Column = 15 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 6, Column = 16 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 6, Column = 17 },

            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 7, Column = 1 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 7, Column = 2 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 7, Column = 3 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 7, Column = 4 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 7, Column = 5 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 7, Column = 6 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 7, Column = 7 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 7, Column = 8 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 7, Column = 9 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 7, Column = 10 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 7, Column = 11 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 7, Column = 12 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 7, Column = 13 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 7, Column = 14 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 7, Column = 15 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 7, Column = 16 },

            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 8, Column = 1 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 8, Column = 2 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 8, Column = 3 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 8, Column = 4 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 8, Column = 5 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 8, Column = 6 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 8, Column = 7 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 8, Column = 8 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 8, Column = 9 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 8, Column = 10 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 8, Column = 11 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 8, Column = 12 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 8, Column = 13 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 8, Column = 14 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 8, Column = 15 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 8, Column = 16 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 8, Column = 17 },

            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 9, Column = 1 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 9, Column = 2 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 9, Column = 3 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 9, Column = 4 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 9, Column = 5 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 9, Column = 6 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 9, Column = 7 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 9, Column = 8 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 9, Column = 9 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 9, Column = 10 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 9, Column = 11 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 9, Column = 12 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 9, Column = 13 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 9, Column = 14 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 9, Column = 15 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 9, Column = 16 },

            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 10, Column = 1 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 10, Column = 2 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 10, Column = 3 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 10, Column = 4 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 10, Column = 5 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 10, Column = 6 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 10, Column = 7 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 10, Column = 8 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 10, Column = 9 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 10, Column = 10 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 10, Column = 11 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 10, Column = 12 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 10, Column = 13 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 10, Column = 14 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 10, Column = 15 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 10, Column = 16 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 10, Column = 17 },

            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 11, Column = 1 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 11, Column = 2 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 11, Column = 3 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 11, Column = 4 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 11, Column = 5 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 11, Column = 6 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 11, Column = 7 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 11, Column = 8 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 11, Column = 9 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 11, Column = 10 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 11, Column = 11 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 11, Column = 12 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 11, Column = 13 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 11, Column = 14 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 11, Column = 15 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 11, Column = 16 },

            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 12, Column = 1 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 12, Column = 2 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 12, Column = 3 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 12, Column = 4 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 12, Column = 5 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 12, Column = 6 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 12, Column = 7 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 12, Column = 8 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 12, Column = 9 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 12, Column = 10 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 12, Column = 11 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 12, Column = 12 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 12, Column = 13 },

            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 13, Column = 1 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 13, Column = 2 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 13, Column = 3 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 13, Column = 4 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 13, Column = 5 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 13, Column = 6 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 13, Column = 7 },
            new SeatEntity { Id = Guid.NewGuid(), TierId = elitMoziNagyteremNezoterTier.Id, Row = 13, Column = 8 }
        };

        var hansJuergenCustomer = new CustomerEntity
        {
            Id = Guid.NewGuid(),
            FirstName = "Hans Jürgen",
            LastName = "Waldmann",
            Email = "hans.juergen.waldmann@gmail.com"
        };

        var susieStressedCustomer = new CustomerEntity
        {
            Id = Guid.NewGuid(),
            FirstName = "Susie",
            LastName = "Stressed",
            Email = "susie.stressed@gmail.com"
        };

        Movies = [
            sleepyHollowMovie,
            iAmLegendMovie,
            melancholiaMovie
        ];

        Genres = [
            darkFantasyGenre,
            slasherHorrorGenre,
            supernaturalHorrorGenre,
            fantasyGenre,
            horrorGenre,
            misteryGenre,
            dystopianSciFiGenre,
            survivalGenre,
            zombieHorrorGenre,
            actionGenre,
            dramaGenre,
            sciFiGenre,
            thrillerGenre,
            psychologicalDramaGenre,
            tragedyGenre
        ];

        Theaters = [
            elitMoziTheater
        ];

        Auditoriums = [
            elitMoziHuszarikTeremAuditorium,
            elitMoziNagyteremAuditorium
        ];

        Tiers = [
            elitMoziHuszarikTeremNezoterTier,
            elitMoziNagyteremErkelyTier,
            elitMoziNagyteremNezoterTier
        ];

        Seats = elitMoziHuszarikTeremNezoterSeats
                    .Concat(elitMoziNagyteremErkelySeats)
                    .Concat(elitMoziNagyteremNezoterSeats)
                    .ToList();

        Customers = [
            hansJuergenCustomer,
            susieStressedCustomer
        ];
    }
}
