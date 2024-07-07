using MediatR;
using Microsoft.EntityFrameworkCore;
using Theater.Application.Modules.ShowDateModule.Commands.UpsertTicketCommand;
using Theater.Application.Repositories;
using Theater.Domain.Models.Entities;

public class UpsertTicketRequestHandler : IRequestHandler<UpsertTicketRequest, bool>
{
    private readonly ITicketRepository _ticketRepository;

    public UpsertTicketRequestHandler(ITicketRepository ticketRepository)
    {
        _ticketRepository = ticketRepository;
    }

    public async Task<bool> Handle(UpsertTicketRequest request, CancellationToken cancellationToken)
    {
        var existingTicket = await _ticketRepository.GetAll()
            .FirstOrDefaultAsync(t => t.SeatId == request.SeatId && t.ShowDateId == request.ShowDateId, cancellationToken);

        if (existingTicket != null)
        {
            // Update existing ticket
            existingTicket.Price = request.Price;
            _ticketRepository.Edit(existingTicket);
        }
        else
        {
            // Add new ticket
            var newTicket = new Ticket
            {
                ShowDateId = request.ShowDateId,
                SeatId = request.SeatId,
                Price = request.Price
            };
            await _ticketRepository.AddAsync(newTicket, cancellationToken);
        }

        await _ticketRepository.SaveAsync(cancellationToken);

        return true;
    }
}
