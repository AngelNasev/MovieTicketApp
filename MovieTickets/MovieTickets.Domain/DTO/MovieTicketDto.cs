namespace MovieTickets.Domain.DTO
{
    public class MovieTicketDto
    {
        public int SeatNumber { get; set; }
        public float Price { get; set; }
        public DateTime Date { get; set; }
        public Guid MovieId { get; set; }
    }
}
