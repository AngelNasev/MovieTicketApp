using Microsoft.AspNetCore.Identity;
using MovieTickets.Domain.DomainModels;

namespace MovieTickets.Domain.Identity
{
    public class AppUser : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public virtual ShoppingCart UserShoppingCart { get; set; }
    }
}
