using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Domain.DomainModels;
using Repository;
using Domain.DTO;
using Services.Interface;
using System.Security.Claims;

namespace Web.Controllers
{
    public class TicketsController : Controller
    {
        private readonly ITicketService _ticketService;

        public TicketsController(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        // GET: Tickets
        public IActionResult Index()
        {
            var dto = new TicketsIndexDto
            {
                Tickets = _ticketService.GetAllTickets(),
                Date = DateTime.Now
            };
            return View(dto);
        }
        
        [HttpPost]
        public IActionResult Index(TicketsIndexDto dto)
        {
            var tickets = _ticketService.GetAllTickets()
                .Where(z => z.DateStart <= dto.Date && z.DateEnd >= dto.Date).ToList();
            var dtoToSend = new TicketsIndexDto
            {
                Tickets = tickets,
                Date = dto.Date
            };
            return View(dtoToSend);
        }

        // GET: Tickets/Details/5
        public IActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = _ticketService.details(id);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // GET: Tickets/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Name,Genre,Price,Rating,DateStart,DateEnd,Id")] Ticket t)
        {
            if (ModelState.IsValid)
            { 
                _ticketService.CreateNewTicket(t);
                return RedirectToAction(nameof(Index));
            }
            return View(t);
        }

        // GET: Tickets/Edit/5
        public IActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = _ticketService.details(id);
            if (ticket == null)
            {
                return NotFound();
            }
            return View(ticket);
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id, [Bind("Name,Genre,Rating,DateStart,DateEnd,Id")] Ticket t)
        {
            if (id != t.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _ticketService.UpdateExistingTicket(t);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TicketExists(t.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(t);
        }

        // GET: Tickets/Delete/5
        public IActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = _ticketService.details(id);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Guid id)
        {
            _ticketService.DeleteTicket(id);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult AddToShoppingCart(Guid? id)
        {
            var model = _ticketService.shoppcardInfo(id);
            return View(model);
        }

        [HttpPost]
        public IActionResult AddToShoppingCart([Bind("TicketId","Quantity")]AddToShoppingCartDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = _ticketService.AddToShoppingCart(dto, userId);
            if(result)
            {
                return RedirectToAction("Index","Tickets");
            }
            return View(dto);
        }

        private bool TicketExists(Guid id)
        {
            if (_ticketService.details(id) != null)
                return true;
            else return false;
        }
    }
}
