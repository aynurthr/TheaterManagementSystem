using MediatR;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
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
        private readonly IActionContextAccessor ctx;

        public PosterBuyTicketRequestHandler(IPosterRepository posterRepository, IActionContextAccessor ctx)
        {
            _posterRepository = posterRepository;
            this.ctx = ctx;

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

            var showDate = poster.ShowDates.FirstOrDefault(sd => sd.Id == request.ShowDateId);
            if (showDate == null)
            {
                return null;
            }

            string host = $"{ctx.ActionContext.HttpContext.Request.Scheme}://{ctx.ActionContext.HttpContext.Request.Host}";

            var response = new PosterBuyTicketResponseDto
            {
                PosterId = poster.Id, // Set PosterId here
                Title = poster.Title,
                ImageSrc = $"{host}/uploads/images/{poster.ImageSrc}",
                ShowDates = poster.ShowDates.Select(sd => new ShowDateDto
                {
                    ShowDateId = sd.Id,
                    Date = sd.Date
                }).ToList(),
                ShowDateId = request.ShowDateId,
                Date = showDate.Date,
                Seats = showDate.Tickets.Select(t => new SeatDto
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
