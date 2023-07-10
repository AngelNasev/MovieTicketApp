using MovieTickets.Domain.Identity;
using System.ComponentModel.DataAnnotations;

namespace MovieTickets.Domain.DomainModels
{
    public class Order : BaseEntity
    {
        public string OwnerId { get; set; }
        public virtual AppUser Owner { get; set; }

        public virtual ICollection<MovieTicket> OrderMovieTickets { get; set; }
    }
}
