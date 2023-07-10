using MovieTickets.Domain.Identity;

namespace MovieTickets.Repository.Interface
{
    public interface IUserRepository
    {
        IEnumerable<AppUser> GetAll();
        AppUser Get(string id);
        void Insert(AppUser entity);
        void Update(AppUser entity);
        void Delete(AppUser entity);
    }
}
