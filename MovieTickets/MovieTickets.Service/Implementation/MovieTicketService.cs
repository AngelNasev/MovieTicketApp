using Microsoft.EntityFrameworkCore;
using MovieTickets.Domain.DomainModels;
using MovieTickets.Domain.DTO;
using MovieTickets.Domain.Enumerations;
using MovieTickets.Repository.Interface;
using MovieTickets.Service.Interface;
using System.Security.Claims;

namespace MovieTickets.Service.Implementation
{
    public class MovieTicketService : IMovieTicketService
    {
        private readonly IMovieTicketRepository _movieTicketRepository;
        private readonly IUserRepository _userRepository;

        public MovieTicketService(IUserRepository userRepository, IMovieTicketRepository movieTicketRepository)
        {
            _userRepository = userRepository;
            _movieTicketRepository = movieTicketRepository;
        }

        public bool AddToShoppingCart(Guid? ticketId, string userID)
        {
            var user = _userRepository.Get(userID);
            var userShoppingCart = user.UserShoppingCart;
            var ticket = _movieTicketRepository.Get(ticketId);
            if (ticket != null)
            {
                if (userShoppingCart.MovieTickets == null)
                {
                    userShoppingCart.MovieTickets = new List<MovieTicket>();
                }
                ticket.ShoppingCartId = userShoppingCart.Id;
                userShoppingCart.MovieTickets.Add(ticket);

                _movieTicketRepository.Update(ticket);
                _userRepository.Update(user);
                return true;
            }
            return false;
        }

        public void CreateNewTicket(MovieTicketDto dto, Movie movie)
        {
            var movieTicket = new MovieTicket
            {
                Id = Guid.NewGuid(),
                SeatNumber = dto.SeatNumber,
                Price = dto.Price,
                Date = dto.Date,
                MovieId = dto.MovieId,
                Movie = movie
            };
            _movieTicketRepository.Insert(movieTicket);
        }

        public void DeleteTicket(Guid id)
        {
            var ticketToDelete = this.GetDetailsForTicket(id);
            _movieTicketRepository.Delete(ticketToDelete);
        }

        public List<MovieTicket> GetTicketsByGenre(string genreString)
        {
            var ticketList = new List<MovieTicket>();
            if (genreString != null)
            {
                Genre genre = (Genre)Enum.Parse(typeof(Genre), genreString);
                ticketList = _movieTicketRepository.GetMovieTicketsByGenre(genre);
            }
            else
            {
                ticketList = _movieTicketRepository.GetAll().ToList();
            }
            return ticketList;
        }

        public List<MovieTicket> GetAllTickets()
        {
            return _movieTicketRepository.GetAll().ToList();
        }

        public MovieTicket GetDetailsForTicket(Guid? id)
        {
            return _movieTicketRepository.Get(id);
        }

        public List<MovieTicket> GetMovieTicketsByDate(DateTime? selectedDate)
        {
            return _movieTicketRepository.GetMovieTicketsByDate(selectedDate);
        }

        public bool TicketExist(Guid? id)
        {
            var ticket = this.GetDetailsForTicket(id);
            
            return ticket != null;
        }

        public void UpdateExistingTicket(EditMovieTicketDto dto, Movie movie)
        {
            var existingMovieTicket = this.GetDetailsForTicket(dto.Id);

            existingMovieTicket.SeatNumber = dto.SeatNumber;
            existingMovieTicket.Price = dto.Price;
            existingMovieTicket.Date = dto.Date;
            existingMovieTicket.MovieId = dto.MovieId;
            existingMovieTicket.Movie = movie;
            _movieTicketRepository.Update(existingMovieTicket);
        }
    }
}
