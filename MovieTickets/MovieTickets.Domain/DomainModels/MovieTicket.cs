using System.ComponentModel.DataAnnotations;

namespace MovieTickets.Domain.DomainModels
{
    public class MovieTicket : BaseEntity
    {
        [Required]
        public int SeatNumber { get; set; }
        [Required]
        public float Price { get; set; }
        [Required]
        public DateTime Date { get; set; }

        public Guid MovieId { get; set; }
        public virtual Movie Movie { get; set; }

        public Guid? ShoppingCartId { get; set; }
        public virtual ShoppingCart? ShoppingCart { get; set; }

        public Guid? OrderId { get; set; }
        public virtual Order? Order { get; set; }
    }
}
