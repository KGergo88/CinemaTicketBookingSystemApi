namespace CinemaTicketBooking.Domain;

// Genre is a flag as movies can be of multiple genres (eg. Horror-Comedy)
[Flags]
public enum Genre
{
    Unknown = 0b_0000_0000,
    Action  = 0b_0000_0001,
    Comedy  = 0b_0000_0010,
    Drama   = 0b_0000_0100,
    Horror  = 0b_0000_1000,
    SciFi   = 0b_0001_0000,
    Short   = 0b_0010_0000
}
