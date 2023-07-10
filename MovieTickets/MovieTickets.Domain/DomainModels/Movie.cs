using MovieTickets.Domain.Enumerations;
using System.ComponentModel.DataAnnotations;

namespace MovieTickets.Domain.DomainModels
{
    public class Movie : BaseEntity
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public Genre Genre { get; set; }
        [Required]
        public int Duration { get; set; }
        [Required]
        public string Poster { get; set; }

        public virtual ICollection<MovieTicket>? MovieTickets { get; set; }
    }
}
