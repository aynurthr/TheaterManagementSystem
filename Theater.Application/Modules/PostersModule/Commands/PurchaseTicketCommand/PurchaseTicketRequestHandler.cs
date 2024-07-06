using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Theater.Application.Repositories;
using Theater.Domain.Models.Entities;
using Theater.Infrastructure.Abstracts;
using Microsoft.EntityFrameworkCore;
using Theater.Application.Modules.PostersModule.Commands.PurchaseTicketCommand;

namespace Theater.Application.Modules.TicketModule.Commands.PurchaseTickets
{
    public class PurchaseTicketRequestHandler : IRequestHandler<PurchaseTicketRequest, bool>
    {
        private readonly IAsyncRepository<Ticket> _ticketRepository;
        private readonly IUserRepository _userRepository;

        public PurchaseTicketRequestHandler(IAsyncRepository<Ticket> ticketRepository, IUserRepository userRepository)
        {
            _ticketRepository = ticketRepository;
            _userRepository = userRepository;
        }

        public async Task<bool> Handle(PurchaseTicketRequest request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetAsync(u => u.Id == request.UserId);
            if (user == null)
            {
                return false;
            }

            var tickets = await _ticketRepository.GetAll(t => request.SeatIds.Contains(t.SeatId) && !t.IsPurchased)
                .ToListAsync(cancellationToken);

            if (tickets.Count != request.SeatIds.Count)
            {
                return false;
            }

            foreach (var ticket in tickets)
            {
                ticket.IsPurchased = true;
                ticket.IsPurchasedBy = user.Id;
                ticket.IsPurchasedAt = DateTime.Now;
                _ticketRepository.Edit(ticket);
            }

            await _ticketRepository.SaveAsync(cancellationToken);
            return true;
        }
    }
}
