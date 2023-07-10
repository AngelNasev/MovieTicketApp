using MovieTickets.Domain.DomainModels;

namespace MovieTickets.Repository.Interface
{
    public interface IOrderRepository
    {
        public List<Order> GetAllOrders();
        public Order GetDetailsForOrder(Guid id);
        public List<Order> GetUserOrders(string userId);
    }
}
