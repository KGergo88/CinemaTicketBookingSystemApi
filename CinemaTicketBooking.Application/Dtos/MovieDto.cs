namespace CinemaTicketBooking.Application.Dtos
{
    public class MovieDto
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public int DurationInSeconds { get; set; }

        public List<string> Genres { get; set; }
    }
}
