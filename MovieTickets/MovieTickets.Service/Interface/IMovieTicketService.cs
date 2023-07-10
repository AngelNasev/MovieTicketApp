using MovieTickets.Domain.DomainModels;
using MovieTickets.Domain.DTO;

namespace MovieTickets.Service.Interface
{
    public interface IMovieTicketService
    {
        List<MovieTicket> GetMovieTicketsByDate(DateTime? selectedDate);
        List<MovieTicket> GetAllTickets();
        MovieTicket GetDetailsForTicket(Guid? id);
        void CreateNewTicket(MovieTicketDto dto, Movie movie);
        void UpdateExistingTicket(EditMovieTicketDto dto, Movie movie);
        void DeleteTicket(Guid id);
        bool AddToShoppingCart(Guid? ticketId, string userID);
        public Boolean TicketExist(Guid? id);
    }
}
