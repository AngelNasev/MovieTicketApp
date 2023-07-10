using MovieTickets.Domain.DomainModels;

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
    }
}
