namespace CinemaTicketBooking.Domain.Entities;

public class Theater
{
    public Guid Id { get; set; }

    public required string Name { get; set; }

    public required string Address { get; set; }

    public required List<Auditorium> Auditoriums { get; set; }
}
