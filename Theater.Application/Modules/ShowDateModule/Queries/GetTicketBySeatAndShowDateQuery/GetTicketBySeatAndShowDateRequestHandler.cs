using MediatR;
using Theater.Application.Modules.ShowDateModule.Queries.GetTicketBySeatAndShowDateQuery;
using Theater.Application.Modules.ShowDateModule.Queries;
using Theater.Application.Repositories;
using Microsoft.EntityFrameworkCore;

public class GetTicketBySeatAndShowDateRequestHandler : IRequestHandler<GetTicketBySeatAndShowDateRequest, TicketDto>
{
    private readonly ITicketRepository _ticketRepository;

    public GetTicketBySeatAndShowDateRequestHandler(ITicketRepository ticketRepository)
    {
        _ticketRepository = ticketRepository;
    }

    public async Task<TicketDto> Handle(GetTicketBySeatAndShowDateRequest request, CancellationToken cancellationToken)
    {
        var ticket = await _ticketRepository.GetAll()
            .FirstOrDefaultAsync(t => t.SeatId == request.SeatId && t.ShowDateId == request.ShowDateId, cancellationToken);

        if (ticket == null)
        {
            return new TicketDto
            {
                ShowDateId = request.ShowDateId,
                SeatId = request.SeatId,
                Price = 0 // Default price for new ticket
            };
        }

        return new TicketDto
        {
            ShowDateId = ticket.ShowDateId,
            SeatId = ticket.SeatId,
            Price = ticket.Price
        };
    }
}
