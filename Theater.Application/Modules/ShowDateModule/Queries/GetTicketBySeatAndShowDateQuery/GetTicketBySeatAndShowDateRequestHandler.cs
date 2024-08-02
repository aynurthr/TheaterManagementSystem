using MediatR;
using Theater.Application.Modules.ShowDateModule.Queries.GetTicketBySeatAndShowDateQuery;
using Theater.Application.Modules.ShowDateModule.Queries;
using Theater.Application.Repositories;
using Microsoft.EntityFrameworkCore;
using Theater.Infrastructure.Exceptions;

public class GetTicketBySeatAndShowDateRequestHandler : IRequestHandler<GetTicketBySeatAndShowDateRequest, TicketDto>
{
    private readonly ITicketRepository _ticketRepository;
    private readonly ISeatRepository _seatRepository;
    private readonly IShowDateRepository _showDateRepository;

    public GetTicketBySeatAndShowDateRequestHandler(
        ITicketRepository ticketRepository,
        ISeatRepository seatRepository,
        IShowDateRepository showDateRepository)
    {
        _ticketRepository = ticketRepository;
        _seatRepository = seatRepository;
        _showDateRepository = showDateRepository;
    }

    public async Task<TicketDto> Handle(GetTicketBySeatAndShowDateRequest request, CancellationToken cancellationToken)
    {
        // Check if the seat or show date is logically deleted
        var seat = await _seatRepository.GetAsync(m => m.Id == request.SeatId && m.DeletedAt == null, cancellationToken);
        if (seat == null)
        {
            return null;
        }

        var showDate = await _showDateRepository.GetAsync(m => m.Id == request.ShowDateId && m.DeletedAt == null, cancellationToken);
        if (showDate == null )
        {
            return null;
        }

        // Fetch the ticket if the seat and show date are valid
        var ticket = await _ticketRepository.GetAll()
            .FirstOrDefaultAsync(t => t.SeatId == request.SeatId && t.ShowDateId == request.ShowDateId && t.DeletedAt == null, cancellationToken);

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
