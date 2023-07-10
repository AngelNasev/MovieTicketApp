using Microsoft.EntityFrameworkCore;
using MovieTickets.Domain.DomainModels;
using MovieTickets.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieTickets.Repository.Implementation
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _context;
        private DbSet<Order> entities;
        string errorMessage = string.Empty;

        public OrderRepository(ApplicationDbContext context)
        {
            _context = context;
            this.entities = _context.Set<Order>();
        }

        public List<Order> GetAllOrders()
        {
            return entities
                .Include(o => o.OrderMovieTickets)
                .Include(o => o.Owner)
                .Include("OrderMovieTickets.Movie")
                .ToList();
        }

        public Order GetDetailsForOrder(Guid id)
        {
            return entities
                .Include(o => o.OrderMovieTickets)
                .Include(o => o.Owner)
                .Include("OrderMovieTickets.Movie")
                .SingleOrDefault(o => o.Id == id);
        }

        public List<Order> GetUserOrders(string userId)
        {
            return entities
                .Include(o => o.OrderMovieTickets)
                .Include(o => o.Owner)
                .Include("OrderMovieTickets.Movie")
                .Where(o => o.OwnerId==userId)
                .ToList();
        }
    }
}
