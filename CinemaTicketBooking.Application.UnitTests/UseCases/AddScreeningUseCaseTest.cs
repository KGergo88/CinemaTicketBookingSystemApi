using CinemaTicketBooking.Application.Interfaces.Repositories;
using CinemaTicketBooking.Application.UseCases;
using CinemaTicketBooking.Domain.Entities;
using Moq;

namespace CinemaTicketBooking.Application.UnitTests.UseCases;

public class AddScreeningUseCaseTest
{
    private readonly Mock<IScreeningRepository> mockScreeningRepository = new();
    private readonly Mock<ITheaterRepository> mockTheaterRepository = new();
    private readonly Mock<IMovieRepository> mockMovieRepository = new();
    private readonly AddScreeningUseCase addScreeningUseCase;

    public AddScreeningUseCaseTest()
    {
        addScreeningUseCase = new(mockScreeningRepository.Object, mockTheaterRepository.Object, mockMovieRepository.Object);
    }

    [Fact]
    public async Task Execute_ThrowsNotFoundException_IfMovieDoesNotExist()
    {
        // Arrange
        mockMovieRepository.Setup(m => m.GetMovieOrNullAsync(It.IsAny<Guid>()))
                           .ReturnsAsync(() => null);

        var expectedScreening = new Screening()
        {
            Id = Guid.NewGuid(),
            AuditoriumId = Guid.NewGuid(),
            MovieId = Guid.NewGuid(),
            Showtime = DateTimeOffset.UtcNow,
            Language = "English",
            Subtitles = "German"
        };

        // Act
        var exception = await Record.ExceptionAsync(async () => await addScreeningUseCase.ExecuteAsync(expectedScreening));

        // Assert
        Assert.NotNull(exception);
        Assert.IsType<Application.Interfaces.UseCases.Exceptions.NotFoundException>(exception);
        Assert.Contains(expectedScreening.MovieId.ToString(), exception.Message);
        mockMovieRepository.Verify(m => m.GetMovieOrNullAsync(It.Is<Guid>(movieId => movieId == expectedScreening.MovieId)), Times.Once);
        mockMovieRepository.VerifyNoOtherCalls();
        mockTheaterRepository.VerifyNoOtherCalls();
        mockScreeningRepository.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task Execute_ThrowsNotFoundException_IfAuditoriumDoesNotExist()
    {
        // Arrange
        var expectedMovie = new Movie()
        {
            Id = Guid.NewGuid(),
            Title = "The Crazies",
            ReleaseYear = 1973,
            Description = "The military attempts to contain a manmade combat virus that causes death and permanent insanity in those infected, as it overtakes a small Pennsylvania town.",
            Duration = TimeSpan.FromMinutes(103),
            Genres = [
                "B-Horror",
                "Dark Comedy",
                "Psychological Horror",
                "Action",
                "Horror",
                "Sci-Fi"
            ]
        };

        mockMovieRepository.Setup(m => m.GetMovieOrNullAsync(It.IsAny<Guid>()))
                           .ReturnsAsync(() => expectedMovie);
        mockTheaterRepository.Setup(m => m.GetAuditoriumOrNullAsync(It.IsAny<Guid>()))
                             .ReturnsAsync(() => null);

        var expectedScreening = new Screening()
        {
            Id = Guid.NewGuid(),
            AuditoriumId = Guid.NewGuid(),
            MovieId = expectedMovie.Id,
            Showtime = DateTimeOffset.UtcNow,
            Language = "English",
            Subtitles = "German"
        };

        // Act
        var exception = await Record.ExceptionAsync(async () => await addScreeningUseCase.ExecuteAsync(expectedScreening));

        // Assert
        Assert.NotNull(exception);
        Assert.IsType<Application.Interfaces.UseCases.Exceptions.NotFoundException>(exception);
        Assert.Contains(expectedScreening.AuditoriumId.ToString(), exception.Message);
        mockMovieRepository.Verify(m => m.GetMovieOrNullAsync(It.Is<Guid>(movieId => movieId == expectedMovie.Id)), Times.Once);
        mockMovieRepository.VerifyNoOtherCalls();
        mockTheaterRepository.Verify(m => m.GetAuditoriumOrNullAsync(It.Is<Guid>(auditoriumId => auditoriumId == expectedScreening.AuditoriumId)), Times.Once);
        mockTheaterRepository.VerifyNoOtherCalls();
        mockScreeningRepository.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task Execute_ThrowsConflictException_IfScreeningOverlapsAnother()
    {
        // Arrange
        var expectedMovie = new Movie()
        {
            Id = Guid.NewGuid(),
            Title = "The Crazies",
            ReleaseYear = 1973,
            Description = "The military attempts to contain a manmade combat virus that causes death and permanent insanity in those infected, as it overtakes a small Pennsylvania town.",
            Duration = TimeSpan.FromMinutes(103),
            Genres = [
                "B-Horror",
                "Dark Comedy",
                "Psychological Horror",
                "Action",
                "Horror",
                "Sci-Fi"
            ]
        };
        var expectedAuditorium = new Auditorium()
        {
            Id = Guid.NewGuid(),
            Name = "Small Auditorium",
            Tiers = [],
            MinimumCleanupDuration = TimeSpan.FromMinutes(15)
        };
        var overlappingScreeningId = Guid.NewGuid();

        mockMovieRepository.Setup(m => m.GetMovieOrNullAsync(It.IsAny<Guid>()))
                           .ReturnsAsync(() => expectedMovie);
        mockTheaterRepository.Setup(m => m.GetAuditoriumOrNullAsync(It.IsAny<Guid>()))
                             .ReturnsAsync(() => expectedAuditorium);
        mockScreeningRepository.Setup(m => m.FindScreeningIdsInTimeFrameAsync(It.IsAny<Guid>(),
                                                                              It.IsAny<DateTimeOffset>(),
                                                                              It.IsAny<TimeSpan>()))
                               .ReturnsAsync(() => [overlappingScreeningId]);

        var expectedScreening = new Screening()
        {
            Id = Guid.NewGuid(),
            AuditoriumId = expectedAuditorium.Id,
            MovieId = expectedMovie.Id,
            Showtime = DateTimeOffset.UtcNow,
            Language = "English",
            Subtitles = "German"
        };
        var expectedTimeFrameStart = expectedScreening.Showtime - expectedAuditorium.MinimumCleanupDuration;
        var expectedTimeframeDuration = expectedMovie.Duration + (expectedAuditorium.MinimumCleanupDuration * 2);

        // Act
        var exception = await Record.ExceptionAsync(async () => await addScreeningUseCase.ExecuteAsync(expectedScreening));

        // Assert
        Assert.NotNull(exception);
        Assert.IsType<Application.Interfaces.UseCases.Exceptions.ConflictException>(exception);
        Assert.Contains(overlappingScreeningId.ToString(), exception.Message);
        mockMovieRepository.Verify(m => m.GetMovieOrNullAsync(It.Is<Guid>(movieId => movieId == expectedMovie.Id)), Times.Once);
        mockMovieRepository.VerifyNoOtherCalls();
        mockTheaterRepository.Verify(m => m.GetAuditoriumOrNullAsync(It.Is<Guid>(auditoriumId => auditoriumId == expectedScreening.AuditoriumId)), Times.Once);
        mockTheaterRepository.VerifyNoOtherCalls();
        mockScreeningRepository.Verify(m => m.FindScreeningIdsInTimeFrameAsync(It.Is<Guid>(auditoriumId => auditoriumId == expectedScreening.AuditoriumId),
                                                                               It.Is<DateTimeOffset>(timeFrameStart => timeFrameStart == expectedTimeFrameStart),
                                                                               It.Is<TimeSpan>(timeFrameDuration => timeFrameDuration == expectedTimeframeDuration)),
                                                                               Times.Once);
        mockScreeningRepository.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task Execute_DoesNotThrow_IfScreeningCanBeStored()
    {
        // Arrange
        var expectedMovie = new Movie()
        {
            Id = Guid.NewGuid(),
            Title = "The Crazies",
            ReleaseYear = 1973,
            Description = "The military attempts to contain a manmade combat virus that causes death and permanent insanity in those infected, as it overtakes a small Pennsylvania town.",
            Duration = TimeSpan.FromMinutes(103),
            Genres = [
                "B-Horror",
                "Dark Comedy",
                "Psychological Horror",
                "Action",
                "Horror",
                "Sci-Fi"
            ]
        };
        var expectedAuditorium = new Auditorium()
        {
            Id = Guid.NewGuid(),
            Name = "Small Auditorium",
            Tiers = [],
            MinimumCleanupDuration = TimeSpan.FromMinutes(15)
        };

        mockMovieRepository.Setup(m => m.GetMovieOrNullAsync(It.IsAny<Guid>()))
                           .ReturnsAsync(() => expectedMovie);
        mockTheaterRepository.Setup(m => m.GetAuditoriumOrNullAsync(It.IsAny<Guid>()))
                             .ReturnsAsync(() => expectedAuditorium);
        mockScreeningRepository.Setup(m => m.FindScreeningIdsInTimeFrameAsync(It.IsAny<Guid>(),
                                                                              It.IsAny<DateTimeOffset>(),
                                                                              It.IsAny<TimeSpan>()))
                               .ReturnsAsync(() => []);

        var expectedScreening = new Screening()
        {
            Id = Guid.NewGuid(),
            AuditoriumId = expectedAuditorium.Id,
            MovieId = expectedMovie.Id,
            Showtime = DateTimeOffset.UtcNow,
            Language = "English",
            Subtitles = "German"
        };
        var expectedTimeFrameStart = expectedScreening.Showtime - expectedAuditorium.MinimumCleanupDuration;
        var expectedTimeframeDuration = expectedMovie.Duration + (expectedAuditorium.MinimumCleanupDuration * 2);

        // Act
        var exception = await Record.ExceptionAsync(async () => await addScreeningUseCase.ExecuteAsync(expectedScreening));

        // Assert
        Assert.Null(exception);
        mockMovieRepository.Verify(m => m.GetMovieOrNullAsync(It.Is<Guid>(movieId => movieId == expectedMovie.Id)), Times.Once);
        mockMovieRepository.VerifyNoOtherCalls();
        mockTheaterRepository.Verify(m => m.GetAuditoriumOrNullAsync(It.Is<Guid>(auditoriumId => auditoriumId == expectedScreening.AuditoriumId)), Times.Once);
        mockTheaterRepository.VerifyNoOtherCalls();
        mockScreeningRepository.Verify(m => m.FindScreeningIdsInTimeFrameAsync(It.Is<Guid>(auditoriumId => auditoriumId == expectedScreening.AuditoriumId),
                                                                               It.Is<DateTimeOffset>(timeFrameStart => timeFrameStart == expectedTimeFrameStart),
                                                                               It.Is<TimeSpan>(timeFrameDuration => timeFrameDuration == expectedTimeframeDuration)),
                                                                               Times.Once);
        mockScreeningRepository.Verify(m => m.AddScreeningsAsync(It.Is<IEnumerable<Screening>>(screenings => screenings.Count() == 1
                                                                                                             && screenings.Contains(expectedScreening))),
                                                                 Times.Once);
        mockScreeningRepository.VerifyNoOtherCalls();
    }
}
