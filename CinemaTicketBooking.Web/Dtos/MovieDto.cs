namespace CinemaTicketBooking.Web.Dtos
{
    public class MovieDto
    {
        public Guid? Id { get; set; }

        public string Title { get; set; }

        public int ReleaseYear { get; set; }

        public string? Description { get; set; }

        public int DurationInSeconds { get; set; }

        public List<string>? Genres { get; set; }
    }
}
