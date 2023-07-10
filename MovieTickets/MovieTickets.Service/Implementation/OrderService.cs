using GemBox.Document;
using MovieTickets.Domain.DomainModels;
using MovieTickets.Repository.Interface;
using MovieTickets.Service.Interface;
using System.IO;
using System.Text;

namespace MovieTickets.Service.Implementation
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            ComponentInfo.SetLicense("FREE-LIMITED-KEY");
            _orderRepository = orderRepository;
        }

        public MemoryStream CreateInvoice(Guid id)
        {
            var result = this._orderRepository.GetDetailsForOrder(id);

            var templatePath = Path.Combine(Directory.GetCurrentDirectory(), "Invoice.docx");

            var document = DocumentModel.Load(templatePath);

            document.Content.Replace("{{OrderNumber}}", result.Id.ToString());
            document.Content.Replace("{{Username}}", result.Owner.Email);

            double totalPrice = result.OrderMovieTickets.Select(u => u.Price).Sum();

            StringBuilder sb = new StringBuilder();

            foreach (var item in result.OrderMovieTickets)
            {
                sb.AppendLine(item.Movie.Name + ", seat: " + item.SeatNumber+ " on "+ item.Date.Date.ToString("MM/dd/yyyy") + " and price of: $" + item.Price);
            }

            document.Content.Replace("{{TicketList}}", sb.ToString());
            document.Content.Replace("{{TotalPrice}}", "$" + totalPrice.ToString());

            var stream = new MemoryStream();

            document.Save(stream, new PdfSaveOptions());

            return stream;
        }

        public List<Order> GetAllOrders()
        {
            return _orderRepository.GetAllOrders();
        }

        public Order GetDetailsForOrder(Guid id)
        {
            return _orderRepository.GetDetailsForOrder(id);
        }

        public List<Order> GetUserOrders(string userId)
        {
            return _orderRepository.GetUserOrders(userId);
        }
    }
}
