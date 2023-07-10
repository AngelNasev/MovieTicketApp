using MovieTickets.Domain.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieTickets.Service.Interface
{
    public interface IOrderService
    {
        public List<Order> GetAllOrders();
        public Order GetDetailsForOrder(Guid id);
        public List<Order> GetUserOrders(string userId);
        public MemoryStream CreateInvoice(Guid id);
    }
}
