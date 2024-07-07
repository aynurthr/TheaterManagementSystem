using MediatR;
using Microsoft.EntityFrameworkCore;
using Theater.Application.Modules.ShowDateModule.Queries;
using Theater.Application.Modules.ShowDateModule.Queries.GetSeatsByShowDateQuery;
using Theater.Application.Repositories;

public class GetSeatsByShowDateRequestHandler : IRequestHandler<GetSeatsByShowDateRequest, IEnumerable<SeatDto>>
{
    private readonly IShowDateRepository _showDateRepository;

    public GetSeatsByShowDateRequestHandler(IShowDateRepository showDateRepository)
    {
        _showDateRepository = showDateRepository;
    }

    public async Task<IEnumerable<SeatDto>> Handle(GetSeatsByShowDateRequest request, CancellationToken cancellationToken)
    {
        var showDate = await _showDateRepository.GetAll()
            .Include(sd => sd.Hall)
                .ThenInclude(h => h.Seats)
                    .ThenInclude(s => s.Tickets)
            .FirstOrDefaultAsync(sd => sd.Id == request.ShowDateId && sd.DeletedAt == null, cancellationToken);

        if (showDate == null)
        {
            return null;
        }

        var seats = showDate.Hall.Seats
            .Select(seat => new SeatDto
            {
                Id = seat.Id,
                Row = seat.Row,
                Number = seat.Number,
                Price = seat.Price,
                IsPurchased = seat.Tickets.Any(t => t.ShowDateId == request.ShowDateId)
            }).ToList();

        return seats;
    }
}
