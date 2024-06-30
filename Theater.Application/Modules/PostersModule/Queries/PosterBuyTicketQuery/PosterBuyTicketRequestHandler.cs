using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Theater.Application.Repositories;
using Theater.Domain.Models.Entities;

namespace Theater.Application.Modules.PosterModule.Queries.PosterBuyTicketQuery
{
    public class PosterBuyTicketRequestHandler : IRequestHandler<PosterBuyTicketRequest, PosterBuyTicketResponseDto>
    {
        private readonly IPosterRepository _posterRepository;

        public PosterBuyTicketRequestHandler(IPosterRepository posterRepository)
        {
            _posterRepository = posterRepository;
        }

        public async Task<PosterBuyTicketResponseDto> Handle(PosterBuyTicketRequest request, CancellationToken cancellationToken)
        {
            var poster = await _posterRepository.GetAll()
                .Where(p => p.Id == request.PosterId)
                .Include(p => p.ShowDates)
                .ThenInclude(sd => sd.Tickets)
                .ThenInclude(t => t.Seat)
                .FirstOrDefaultAsync(cancellationToken);

            if (poster == null)
            {
                return null;
            }

            var response = new PosterBuyTicketResponseDto
            {
                Title = poster.Title,
                ImageSrc = poster.ImageSrc,
                ShowDates = poster.ShowDates.Select(sd => new ShowDateDto
                {
                    ShowDateId = sd.Id,
                    Date = sd.Date
                }).ToList(),
                ShowDateId = request.ShowDateId,
                Date = poster.ShowDates.FirstOrDefault(sd => sd.Id == request.ShowDateId)?.Date ?? DateTime.MinValue,
                Seats = poster.ShowDates.FirstOrDefault(sd => sd.Id == request.ShowDateId)?.Tickets.Select(t => new SeatDto
                {
                    Row = t.Seat.Row,
                    SeatNumber = t.Seat.Number,
                    IsPurchased = t.IsPurchased,
                    Price = t.Price
                }).ToList()
            };

            return response;
        }
    }
}
