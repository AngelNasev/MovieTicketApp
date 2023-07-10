using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieTickets.Domain.DomainModels;
using MovieTickets.Domain.DTO;
using MovieTickets.Domain.Identity;
using MovieTickets.Service.Interface;
using Stripe;
using System.Net.Sockets;
using System.Security.Claims;

namespace MovieTickets.Web.Controllers
{
    [Authorize]
    public class ShoppingCartController : Controller
    {
        
        private readonly IShoppingCartService _shoppingCartService;
        public ShoppingCartController(IShoppingCartService shoppingCartService)
        {
            _shoppingCartService = shoppingCartService;
        }

        public IActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return View(_shoppingCartService.getShoppingCartInfo(userId));
        }

        public IActionResult Delete(Guid id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!string.IsNullOrEmpty(userId))
            {
                _shoppingCartService.deleteTicketFromShoppingCart(userId,id);
            }

            return RedirectToAction("Index", "ShoppingCart");
        }

        public IActionResult Order(string stripeEmail, string stripeToken)
        {
            var customerService = new CustomerService();
            var chargeService = new ChargeService();
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var customer = customerService.Create(new CustomerCreateOptions
            {
                Email = stripeEmail,
                Source = stripeToken
            });
            var TotalPrice = _shoppingCartService.getShoppingCartInfo(userId).TotalPrice;
            var charge = chargeService.Create(new ChargeCreateOptions
            {
                Amount = Convert.ToInt32(TotalPrice * 100),
                Description = "Movie Ticket Payment",
                Currency = "usd",
                Customer = customer.Id
            });

            if (charge.Status == "succeeded")
            {
                _shoppingCartService.order(userId);
                return RedirectToAction("Index", "Order");

            }

            return RedirectToAction("Index", "ShoppingCart");
        }
    }
}
