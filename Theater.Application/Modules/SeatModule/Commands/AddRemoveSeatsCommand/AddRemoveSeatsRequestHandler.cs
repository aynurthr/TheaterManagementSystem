using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Theater.Application.Repositories;
using Theater.Domain.Models.Entities;

namespace Theater.Application.Modules.SeatModule.Commands.AddRemoveSeatsCommand
{
    public class AddRemoveSeatsRequestHandler : IRequestHandler<AddRemoveSeatsRequest, bool>
    {
        private readonly ISeatRepository _seatRepository;
        private readonly IHallRepository _hallRepository;
        private readonly ITicketRepository _ticketRepository;

        public AddRemoveSeatsRequestHandler(ISeatRepository seatRepository, IHallRepository hallRepository, ITicketRepository ticketRepository)
        {
            _seatRepository = seatRepository;
            _hallRepository = hallRepository;
            _ticketRepository = ticketRepository;
        }

        public async Task<bool> Handle(AddRemoveSeatsRequest request, CancellationToken cancellationToken)
        {
            var hall = await _hallRepository.GetAsync(h => h.Id == request.HallId);
            if (hall == null)
            {
                return false;
            }

            var existingSeats = await _seatRepository.GetAll(s => s.HallId == request.HallId).ToListAsync(cancellationToken);

            foreach (var seat in request.SeatsToAdd)
            {
                var existingSeat = existingSeats.FirstOrDefault(es => es.Row == seat.Row && es.Number == seat.Number);

                if (existingSeat != null)
                {
                    // If the seat exists and is soft-deleted, reset DeletedAt and DeletedBy
                    if (existingSeat.DeletedAt != null)
                    {
                        existingSeat.DeletedAt = null;
                        existingSeat.DeletedBy = null;
                        _seatRepository.Edit(existingSeat);

                        // Undelete the associated tickets
                        var tickets = await _ticketRepository.GetAll(t => t.SeatId == existingSeat.Id).ToListAsync(cancellationToken);
                        foreach (var ticket in tickets)
                        {
                            ticket.DeletedAt = null;
                            ticket.DeletedBy = null;
                            _ticketRepository.Edit(ticket);
                        }
                    }
                }
                else
                {
                    // Add new seat if it doesn't exist
                    var newSeat = new Seat
                    {
                        HallId = seat.HallId,
                        Row = seat.Row,
                        Number = seat.Number,
                        Price = 10 // Default price, we wont need it since tickets already have a price
                    };
                    await _seatRepository.AddAsync(newSeat, cancellationToken);
                }
            }

            foreach (var seat in request.SeatsToRemove)
            {
                var existingSeat = existingSeats.FirstOrDefault(s => s.Row == seat.Row && s.Number == seat.Number);
                if (existingSeat != null)
                {
                    // Remove associated tickets
                    var tickets = await _ticketRepository.GetAll(t => t.SeatId == existingSeat.Id).ToListAsync(cancellationToken);
                    foreach (var ticket in tickets)
                    {
                        _ticketRepository.Remove(ticket);
                    }

                    _seatRepository.Remove(existingSeat);
                }
            }

            await _seatRepository.SaveAsync(cancellationToken);
            await _ticketRepository.SaveAsync(cancellationToken);

            return true;
        }
    }
}
