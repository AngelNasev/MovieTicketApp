using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MovieTickets.Domain.DomainModels;
using MovieTickets.Domain.Identity;

namespace MovieTickets.Repository
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }

        public virtual DbSet<Movie> Movies { get; set; }
        public virtual DbSet<MovieTicket> MovieTickets { get; set; }
        public virtual DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<EmailMessage> EmailMessages { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<ShoppingCart>()
                .HasOne<AppUser>(x => x.Owner)
                .WithOne(x => x.UserShoppingCart)
                .HasForeignKey<ShoppingCart>(x => x.OwnerId);

            builder.Entity<MovieTicket>()
                .HasOne<Movie>(mt => mt.Movie)
                .WithMany(m => m.MovieTickets)
                .HasForeignKey(mt => mt.MovieId);

            builder.Entity<MovieTicket>()
                .HasOne<ShoppingCart>(mt => mt.ShoppingCart)
                .WithMany(s => s.MovieTickets)
                .HasForeignKey(mt => mt.ShoppingCartId)
                .IsRequired(false);

            builder.Entity<MovieTicket>()
                .HasOne<Order>(mt => mt.Order)
                .WithMany(s => s.OrderMovieTickets)
                .HasForeignKey(mt => mt.OrderId)
                .IsRequired(false);

            base.OnModelCreating(builder);
        }
    }
}