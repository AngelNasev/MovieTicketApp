using Microsoft.EntityFrameworkCore;
using MovieTickets.Domain.Identity;
using MovieTickets.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieTickets.Repository.Implementation
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        private DbSet<AppUser> entities;
        string errorMessage = string.Empty;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
            entities = _context.Set<AppUser>();
        }

        public void Delete(AppUser entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Remove(entity);
            _context.SaveChanges();
        }

        public AppUser Get(string id)
        {
            return entities
                .Include(u => u.UserShoppingCart)
                .Include("UserShoppingCart.MovieTickets")
                .Include("UserShoppingCart.MovieTickets.Movie")
                .SingleOrDefault(u => u.Id == id);
        }

        public IEnumerable<AppUser> GetAll()
        {
            return entities.AsEnumerable();
        }

        public void Insert(AppUser entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Add(entity);
            _context.SaveChanges();
        }

        public void Update(AppUser entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Update(entity);
            _context.SaveChanges();
        }
    }
}
