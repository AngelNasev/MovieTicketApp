using Microsoft.EntityFrameworkCore;
using MovieTickets.Domain.DomainModels;
using MovieTickets.Domain.Enumerations;
using MovieTickets.Repository.Interface;

namespace MovieTickets.Repository.Implementation
{
    public class MovieTicketRepository : IMovieTicketRepository
    {
        private readonly ApplicationDbContext _context;
        private DbSet<MovieTicket> entities;
        string errorMessage = string.Empty;

        public MovieTicketRepository(ApplicationDbContext context)
        {
            _context = context;
            entities = _context.Set<MovieTicket>();
        }

        public MovieTicket Delete(MovieTicket entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Remove(entity);
            _context.SaveChanges();
            return entity;
        }

        public MovieTicket Get(Guid? id)
        {
            return entities
                .Include(x => x.Movie)
                .SingleOrDefault(s => s.Id == id);
        }

        public IEnumerable<MovieTicket> GetAll()
        {
            return entities
                .Include(x => x.Movie)
                .AsEnumerable();
        }

        public List<MovieTicket> GetMovieTicketsByDate(DateTime? selectedDate)
        {
            IQueryable<MovieTicket> tickets = _context.MovieTickets.Include(m => m.Movie);

            if (selectedDate != null)
            {
                tickets = tickets.Where(t => t.Date.Date == selectedDate);
            }
            return tickets.ToList();
        }

        public List<MovieTicket> GetMovieTicketsByGenre(Genre genre)
        {
            IQueryable<MovieTicket> tickets = _context.MovieTickets
                .Include(m => m.Movie)
                .Where(m => m.Movie.Genre == genre);
            return tickets.ToList();
        }

        public MovieTicket Insert(MovieTicket entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Add(entity);
            _context.SaveChanges();

            return entity;
        }

        public MovieTicket Update(MovieTicket entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Update(entity);
            _context.SaveChanges();
            return entity;
        }
    }
}
