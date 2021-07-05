using Domain.DomainModels;
using Domain.DTO;
using Repository.Interface;
using Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.Implementation
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IRepository<ShoppingCart> _shoppingCartRepositorty;
        private readonly IUserRepository _userRepository;

        public ShoppingCartService(IRepository<ShoppingCart> shoppingCartRepository, 
            IUserRepository userRepository)
        {
            _shoppingCartRepositorty = shoppingCartRepository;
            _userRepository = userRepository;
      
        }

        public bool deleteTicket(string userId, Guid id)
        {
            if (!string.IsNullOrEmpty(userId) && id != null)
            {
                var loggeduser = this._userRepository.Get(userId);
                var shoppcard = loggeduser.UserCart;
                var fordelete = shoppcard.Tickets.Where(z => z.TicketId.Equals(id)).FirstOrDefault();
                shoppcard.Tickets.Remove(fordelete);
                this._shoppingCartRepositorty.Update(shoppcard);
                return true;
            }

            return false;
        }

        public ShoppingCartDto shoppcardInfo(string userId)
        {
            var loggeduser = this._userRepository.Get(userId);

            var shoppcard = loggeduser.UserCart;

            var tickets = shoppcard.Tickets.ToList();

            var allTicketsPrice = tickets.Select(z => new
            {
                TicketPrice = z.Ticket.Price,
                Quantity = z.Quantity
            }).ToList();

            var totalPrice = 0.0;


            foreach (var item in allTicketsPrice)
            {
                totalPrice += item.Quantity * item.TicketPrice;
            }


            ShoppingCartDto scard = new ShoppingCartDto
            {
                Tickets = tickets,
                TotalPrice = totalPrice
            };


            return scard;

        }

       
    }
}
