using AutoMapper;
using CinemaTicketBooking.Application.Interfaces.UseCases;
using CinemaTicketBooking.Application.Interfaces.UseCases.Exceptions;
using CinemaTicketBooking.Domain.Entities;
using CinemaTicketBooking.Web;
using CinemaTicketBooking.Web.Dtos;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Net.Http.Json;

namespace CinemaTicketBooking.IntegrationTests.Web.Controllers;

public class ScreeningsControllerTests
{
    private const string screeningscontrollerRoute = "/api/screenings";

    private readonly IMapper mapper;
    private readonly HttpClient httpClient;

    private readonly Mock<IAddScreeningUseCase> mockAddScreeningsUseCase = new();
    private readonly Mock<ISetPricingUseCase> mockSetPricingUseCase = new();
    private readonly Mock<IGetAvailableSeatsUseCase> mockGetAvailableSeatsUseCase = new();

    public ScreeningsControllerTests()
    {
        var mappingConfig = new MapperConfiguration(mc =>
        {
            mc.AddProfile(new MappingProfile());
        });
        mapper = mappingConfig.CreateMapper();

        var factory = new CinemaTicketBookingWebApplicationFactory(services =>
        {
            services.AddSingleton(mockAddScreeningsUseCase.Object)
                    .AddSingleton(mockSetPricingUseCase.Object)
                    .AddSingleton(mockGetAvailableSeatsUseCase.Object); ;
        });

        httpClient = factory.CreateClient();
    }

    [Fact]
    public async Task AddScreening_ReturnsNotFound_WhenUseCaseThrowsNotFoundException()
    {
        // Arrange
        mockAddScreeningsUseCase.Setup(x => x.ExecuteAsync(It.IsAny<Screening>()))
                                .ThrowsAsync(new NotFoundException("Auditorium does not exist"));
        var screeningDto = new ScreeningDto
        {
            AuditoriumId = Guid.NewGuid(),  // Not existing ID
            MovieId = Guid.NewGuid(),       // Not existing ID
            ShowTime = DateTimeOffset.UtcNow,
            Language = "English",
            Subtitles = "French"
        };

        // Act
        var response = await httpClient.PostAsJsonAsync(screeningscontrollerRoute, screeningDto);

        // Assert
        Assert.Equal(404, (int)response.StatusCode);
        mockAddScreeningsUseCase.Verify(
            mock => mock.ExecuteAsync(It.Is<Screening>(s => s.AuditoriumId == screeningDto.AuditoriumId && s.MovieId == screeningDto.MovieId)),
            Times.Once);
        mockAddScreeningsUseCase.VerifyNoOtherCalls();
    }
}
