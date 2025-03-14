using CinemaTicketBooking.Application.Interfaces.Repositories;
using CinemaTicketBooking.Application.Interfaces.UseCases;
using CinemaTicketBooking.Application.UseCases;
using CinemaTicketBooking.Domain.Entities;
using Moq;

namespace CinemaTicketBooking.Application.UnitTests.UseCases;

public class MakeBookingUseCaseTest
{
    private readonly Mock<IBookingRepository> mockBookingRepository = new();
    private readonly Mock<ICustomerRepository> mockCustomerRepository = new();
    private readonly Mock<IScreeningRepository> mockScreeningRepository = new();
    private readonly Mock<ISeatReservationRepository> mockSeatReservationRepository = new();

    #region ExecuteAsync Tests

    [Fact]
    public async Task UnknownCustomersAreRejectedAsync()
    {
        // Arrange
        mockCustomerRepository.Setup(
            mcr => mcr.GetCustomerOrNullAsync(
                It.IsAny<Guid>())).ReturnsAsync(null as Customer);
        var makeBookingUseCase = new MakeBookingUseCase(mockBookingRepository.Object,
                                                        mockCustomerRepository.Object,
                                                        mockScreeningRepository.Object,
                                                        mockSeatReservationRepository.Object);
        var customerId = Guid.NewGuid();

        // Act
        var exception = await Record.ExceptionAsync(
            () => makeBookingUseCase.ExecuteAsync(customerId, Guid.NewGuid(), [])
        );

        // Assert
        Assert.IsType<MakeBookingException>(exception);
        Assert.Equal($"Unknown customer Id: {customerId}", exception.Message);
    }

    [Fact]
    public async Task UnknownScreeningsAreRejectedAsync()
    {
        // Arrange
        mockCustomerRepository.Setup(
            mcr => mcr.GetCustomerOrNullAsync(
                It.IsAny<Guid>())).ReturnsAsync(
                    (Guid id) => new Customer()
                    {
                        Id = id,
                        FirstName = "Hans Jürgen",
                        LastName = "Waldmann",
                        Email = "hans.juergen.waldmann@gmail.com"
                    }
            );
        var makeBookingUseCase = new MakeBookingUseCase(mockBookingRepository.Object,
                                                        mockCustomerRepository.Object,
                                                        mockScreeningRepository.Object,
                                                        mockSeatReservationRepository.Object);
        var screeningId = Guid.NewGuid();

        // Act
        var exception = await Record.ExceptionAsync(
            () => makeBookingUseCase.ExecuteAsync(Guid.NewGuid(), screeningId, [])
        );

        // Assert
        Assert.IsType<MakeBookingException>(exception);
        Assert.Equal($"Unknown screening Id: {screeningId}", exception.Message);
    }

    public static IEnumerable<object?[]> PastScreeningsCannotBeBookedAsyncData()
    {
        // Yesterday's screening. This shall be not bookable anymore.
        yield return
        [
            DateTimeOffset.UtcNow - TimeSpan.FromDays(1),
            new MakeBookingException("The screening's showtime is in the past, thus not bookable.")
        ];

        // Tomorrow's screening. This can still be booked!
        yield return
        [
            DateTimeOffset.UtcNow + TimeSpan.FromDays(1),
            null
        ];
    }

    [Theory]
    [MemberData(nameof(PastScreeningsCannotBeBookedAsyncData))]
    public async Task PastScreeningsCannotBeBookedAsync(DateTimeOffset showTime, MakeBookingException? expectedException)
    {
        // Arrange
        mockCustomerRepository.Setup(
            mcr => mcr.GetCustomerOrNullAsync(
                It.IsAny<Guid>())).ReturnsAsync(
                    (Guid id) => new Customer()
                    {
                        Id = id,
                        FirstName = "Hans Jürgen",
                        LastName = "Waldmann",
                        Email = "hans.juergen.waldmann@gmail.com"
                    }
            );
        mockScreeningRepository.Setup(
            msr => msr.GetScreeningOrNullAsync(
                It.IsAny<Guid>())).ReturnsAsync(
                    (Guid id) => new Screening()
                    {
                        Id = id,
                        AuditoriumId = Guid.NewGuid(),
                        MovieId = Guid.NewGuid(),
                        Showtime = showTime,
                        Language = "German"
                    }
            );
        var makeBookingUseCase = new MakeBookingUseCase(mockBookingRepository.Object,
                                                        mockCustomerRepository.Object,
                                                        mockScreeningRepository.Object,
                                                        mockSeatReservationRepository.Object);

        // Act
        var exception = await Record.ExceptionAsync(
            () => makeBookingUseCase.ExecuteAsync(Guid.NewGuid(), Guid.NewGuid(), [])
        );

        // Assert
        if (expectedException is not null)
        {
            Assert.IsType<MakeBookingException>(exception);
            Assert.Equal("The screening's showtime is in the past, thus not bookable.", exception.Message);
        }
        else
        {
            Assert.Null(exception);
        }
    }

    [Fact]
    public async Task SeatReservationRepositoryExceptionsAreHandledAsync()
    {
        // Arrange
        mockCustomerRepository.Setup(
            mcr => mcr.GetCustomerOrNullAsync(
                It.IsAny<Guid>())).ReturnsAsync(
                    (Guid id) => new Customer()
                    {
                        Id = id,
                        FirstName = "Hans Jürgen",
                        LastName = "Waldmann",
                        Email = "hans.juergen.waldmann@gmail.com"
                    }
            );
        mockScreeningRepository.Setup(
            msr => msr.GetScreeningOrNullAsync(
                It.IsAny<Guid>())).ReturnsAsync(
                    (Guid id) => new Screening()
                    {
                        Id = id,
                        AuditoriumId = Guid.NewGuid(),
                        MovieId = Guid.NewGuid(),
                        Showtime = DateTimeOffset.UtcNow + TimeSpan.FromDays(1),
                        Language = "German"
                    }
            );
        var seatReservationRepositoryExceptionMessage = "Some known exception from the repository.";
        mockSeatReservationRepository.Setup(
            msrr => msrr.AddSeatReservationsAsync(
                It.IsAny<IEnumerable<Guid>>(), It.IsAny<Guid>(), It.IsAny<Guid>())).ThrowsAsync(
                    new SeatReservationRepositoryException(seatReservationRepositoryExceptionMessage)
            );
        var makeBookingUseCase = new MakeBookingUseCase(mockBookingRepository.Object,
                                                        mockCustomerRepository.Object,
                                                        mockScreeningRepository.Object,
                                                        mockSeatReservationRepository.Object);

        // Act
        var exception = await Record.ExceptionAsync(
            () => makeBookingUseCase.ExecuteAsync(Guid.NewGuid(), Guid.NewGuid(), [])
        );

        // Assert
        Assert.IsType<MakeBookingException>(exception);
        Assert.Equal($"Could not reserve seats! Error: \"{seatReservationRepositoryExceptionMessage}\"", exception.Message);
    }

    #endregion
}
