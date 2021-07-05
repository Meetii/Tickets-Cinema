using Domain.DomainModels;
using Domain.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Interface
{
    public interface ITicketService
    {
        List<Ticket> GetAllTickets();
        Ticket details(Guid? id);
        void CreateNewTicket(Ticket t);
        void UpdateExistingTicket(Ticket t);
        AddToShoppingCartDto shoppcardInfo(Guid? id);
        void DeleteTicket(Guid id);
        bool AddToShoppingCart(AddToShoppingCartDto item, string userID);
    }
}
