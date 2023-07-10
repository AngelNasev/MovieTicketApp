using MovieTickets.Domain.DomainModels;
using MovieTickets.Domain.Enumerations;

namespace MovieTickets.Repository.Interface
{
    public interface IMovieTicketRepository
    {
        IEnumerable<MovieTicket> GetAll();
        MovieTicket Get(Guid? id);
        MovieTicket Insert(MovieTicket entity);
        MovieTicket Update(MovieTicket entity);
        MovieTicket Delete(MovieTicket entity);
        List<MovieTicket> GetMovieTicketsByDate(DateTime? selectedDate);
        List<MovieTicket> GetMovieTicketsByGenre(Genre genre);
    }
}
