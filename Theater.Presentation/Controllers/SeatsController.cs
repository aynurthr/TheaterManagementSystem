using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Theater.Application.Repositories;
using Theater.Presentation.Models;
using System.Collections.Generic;

namespace Theater.Presentation.Controllers
{
    public class SeatsController : Controller
    {
        private readonly IPosterRepository _posterRepository;
        private readonly ISeatRepository _seatRepository;

        public SeatsController(IPosterRepository posterRepository, ISeatRepository seatRepository)
        {
            _posterRepository = posterRepository;
            _seatRepository = seatRepository;
        }

        [HttpGet("seats/{showDateId}")]
        public async Task<IActionResult> GetSeats(int showDateId)
        {
            var showDate = await _posterRepository.GetShowDateByIdAsync(showDateId, CancellationToken.None);
            if (showDate == null)
            {
                return NotFound();
            }

            var poster = await _posterRepository.GetByIdAsync(showDate.PosterId, CancellationToken.None);
            if (poster == null)
            {
                return NotFound();
            }

            var seats = await _seatRepository.GetSeatsByPosterIdAsync(showDate.PosterId, CancellationToken.None);
            var reservedSeatIds = await _seatRepository.GetReservedSeatIdsByShowDateIdAsync(showDateId, CancellationToken.None);

            foreach (var seat in seats)
            {
                seat.IsReserved = reservedSeatIds.Any(rs => rs == seat.Row * 100 + seat.Number); // Assuming each seat has a unique ID based on row and number
            }

            var model = new SeatingChartViewModel
            {
                ShowDateId = showDateId,
                Seats = seats.Select(s => new SeatDto
                {
                    Row = s.Row,
                    Number = s.Number,
                    Price = s.Price,
                    IsReserved = s.IsReserved
                }).ToList(),
                ShowDates = new List<ShowDateDto>
                {
                    new ShowDateDto
                    {
                        Id = showDate.Id,
                        Date = showDate.Date
                    }
                },
                Title = poster.Title
            };

            return View(model);
        }
    }
}
