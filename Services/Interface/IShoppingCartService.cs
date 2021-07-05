using Domain.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Interface
{
    public interface IShoppingCartService
    {
        ShoppingCartDto shoppcardInfo(string userId);
        bool deleteTicket(string userId, Guid id);
    }
}
