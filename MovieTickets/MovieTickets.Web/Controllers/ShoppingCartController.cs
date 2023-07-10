using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieTickets.Domain.DomainModels;
using MovieTickets.Domain.DTO;
using MovieTickets.Domain.Identity;
using MovieTickets.Service.Interface;
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

        public IActionResult Order()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!string.IsNullOrEmpty(userId))
            {
                _shoppingCartService.order(userId);
            }

            return RedirectToAction("Index", "ShoppingCart");
        }
    }
}
