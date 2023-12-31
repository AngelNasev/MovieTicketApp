﻿using Microsoft.EntityFrameworkCore;
using MovieTickets.Domain.DomainModels;
using MovieTickets.Domain.DTO;
using MovieTickets.Repository.Interface;
using MovieTickets.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieTickets.Service.Implementation
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMovieTicketRepository _movieTicketRepository;
        private readonly IRepository<ShoppingCart> _shoppingCartRepository;
        private readonly IRepository<Order> _orderRepository;
        private readonly IRepository<EmailMessage> _emailMessageRepository;
       

        public ShoppingCartService(IUserRepository userRepository, IMovieTicketRepository movieTicketRepository, IRepository<ShoppingCart> shoppingCartRepository, IRepository<Order> orderRepository, IRepository<EmailMessage> emailMessageRepository)
        {
            _userRepository = userRepository;
            _movieTicketRepository = movieTicketRepository;
            _shoppingCartRepository = shoppingCartRepository;
            _orderRepository = orderRepository;
            _emailMessageRepository = emailMessageRepository;
        }

        public bool deleteTicketFromShoppingCart(string userId, Guid ticketId)
        {
            if (!string.IsNullOrEmpty(userId) && ticketId != null)
            {
                var loggedInUser = _userRepository.Get(userId);
                var userShoppingCart = loggedInUser.UserShoppingCart;

                var ticket = userShoppingCart.MovieTickets.Where(m => m.Id == ticketId).FirstOrDefault();
                ticket.ShoppingCartId = null;
                ticket.ShoppingCart = null;
                userShoppingCart.MovieTickets.Remove(ticket);

                _movieTicketRepository.Update(ticket);
                _shoppingCartRepository.Update(userShoppingCart);
                return true;
            }
            return false;
        }

        public ShoppingCartDto getShoppingCartInfo(string userId)
        {
            if(userId != null)
            {
                var loggedInUser = _userRepository.Get(userId);
                
                var userShoppingCart = loggedInUser.UserShoppingCart;

                double totalPrice = userShoppingCart.MovieTickets.Select(u => u.Price).Sum();

                var tickets = userShoppingCart.MovieTickets.ToList();

                ShoppingCartDto item = new ShoppingCartDto
                {
                    MovieTickets = tickets,
                    TotalPrice = totalPrice,
                };
                return item;
            }
            return new ShoppingCartDto();
            
        }

        public bool order(string userId)
        {
            if (!string.IsNullOrEmpty(userId))
            {
                var loggedInUser = _userRepository.Get(userId);

                var userShoppingCart = loggedInUser.UserShoppingCart;

                EmailMessage mail = new EmailMessage();
                mail.MailTo = loggedInUser.Email;
                mail.Subject = "Sucessfuly created order!";
                mail.Status = false;

                Order userOrder = new Order
                {
                    Id = Guid.NewGuid(),
                    OwnerId = loggedInUser.Id,
                    Owner = loggedInUser
                };

                List<MovieTicket> ticketsInOrder = new List<MovieTicket>();
                ticketsInOrder = userShoppingCart.MovieTickets.ToList();
                userOrder.OrderMovieTickets = new List<MovieTicket>();

                StringBuilder sb = new StringBuilder();
                sb.AppendLine("Your order is completed. The order conatins: ");
                for (int i = 1; i <= ticketsInOrder.Count; i++)
                {
                    var item = ticketsInOrder[i-1];
                    sb.AppendLine(i.ToString()+". "+item.Movie.Name + ", seat: " + item.SeatNumber + " on " + item.Date.Date.ToString("MM/dd/yyyy") + " and price of: $" + item.Price+"\n");
                }
                double totalPrice = ticketsInOrder.Select(u => u.Price).Sum();
                sb.AppendLine("Total the price for your order: $" + totalPrice.ToString());
                mail.Body = sb.ToString();
                _emailMessageRepository.Insert(mail);

                foreach (var ticket in ticketsInOrder)
                {
                    userOrder.OrderMovieTickets.Add(ticket);
                }

                _orderRepository.Insert(userOrder);

                foreach (var ticket in ticketsInOrder)
                {
                    ticket.OrderId = userOrder.Id;
                    ticket.Order = userOrder;
                    ticket.ShoppingCartId = null;
                    ticket.ShoppingCart = null;
                    _movieTicketRepository.Update(ticket);
                }

                loggedInUser.UserShoppingCart.MovieTickets.Clear();
                _userRepository.Update(loggedInUser);
                return true;
            }
            return false;
        }
    }
}
