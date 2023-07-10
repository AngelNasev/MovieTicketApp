using MovieTickets.Domain.DomainModels;

namespace MovieTickets.Domain.DTO
{
    public class ShoppingCartDto
    {
        public List<MovieTicket> MovieTickets { get; set; }
        public double TotalPrice { get; set; }
    }
}
