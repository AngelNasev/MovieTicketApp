using GemBox.Document;
using Microsoft.AspNetCore.Mvc;
using MovieTickets.Domain.DomainModels;
using MovieTickets.Service.Interface;
using System.Security.Claims;

namespace MovieTickets.Web.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public IActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return View(_orderService.GetUserOrders(userId));
        }
        public IActionResult Details(Guid id) 
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return View(_orderService.GetDetailsForOrder(id));
        }
        public IActionResult CreateInvoice(Guid id)
        {
            var stream = _orderService.CreateInvoice(id);

            return File(stream.ToArray(), new PdfSaveOptions().ContentType, "ExportOrderInvoice.pdf");
        }
    }
}
