using MovieTickets.Domain.Identity;
using System.ComponentModel.DataAnnotations;

namespace MovieTickets.Domain.DomainModels
{
    public class ShoppingCart : BaseEntity
    {
        public string OwnerId { get; set; }
        public virtual AppUser Owner { get; set; }

        public virtual ICollection<MovieTicket> MovieTickets { get; set; }

    }
}
