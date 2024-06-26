using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Theater.Application.Modules.PosterModule.Queries.PosterBuyTicketRequestDto;
using Theater.Application.Repositories;
using Theater.Domain.Models.Entities;
using Theater.Infrastructure.Exceptions;

namespace Theater.Application.Modules.PosterModule.Queries.PosterBuyTicketRequestQuery
{
    public class PosterBuyTicketRequestHandler : IRequestHandler<PosterBuyTicketRequest, SeatingChartViewModel>
    {
        private readonly IPosterRepository _posterRepository;
        private readonly ISeatRepository _seatRepository;

        public PosterBuyTicketRequestHandler(IPosterRepository posterRepository, ISeatRepository seatRepository)
        {
            _posterRepository = posterRepository;
            _seatRepository = seatRepository;
        }

        public async Task<SeatingChartViewModel> Handle(PosterBuyTicketRequest request, CancellationToken cancellationToken)
        {
            var poster = await _posterRepository.GetByIdAsync(request.PosterId, cancellationToken);
            if (poster == null)
            {
                throw new NotFoundException(nameof(Poster), request.PosterId);
            }

            var showDates = await _posterRepository.GetShowDatesByPosterIdAsync(request.PosterId, cancellationToken);
            var seats = await _seatRepository.GetSeatsByShowDateIdAsync(request.ShowDateId, cancellationToken);
            var reservedSeatIds = await _seatRepository.GetReservedSeatIdsByShowDateIdAsync(request.ShowDateId, cancellationToken);

            return new SeatingChartViewModel
            {
                Title = poster.Title,
                ShowDates = showDates.Select(sd => new ShowDateDto
                {
                    Id = sd.Id,
                    Date = sd.Date
                }).ToList(),
                Seats = seats.Select(s => new SeatDto
                {
                    Row = s.Row,
                    Number = s.Number,
                    Price = s.Price,
                    IsReserved = reservedSeatIds.Contains(s.Id)
                }).ToList()
            };
        }
    }
}
