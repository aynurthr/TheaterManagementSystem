using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Theater.Application.Modules.PosterModule.Queries.PosterBuyTicketRequestDto;
using Theater.Application.Repositories;
using Theater.Domain.Models.Entities;

namespace Theater.Application.Modules.HallModule.Queries
{
    public class HallGetByIdRequestHandler : IRequestHandler<HallGetByIdRequest, HallRequestDto>
    {
        private readonly IHallRepository hallRepository;

        public HallGetByIdRequestHandler(IHallRepository hallRepository)
        {
            this.hallRepository = hallRepository;
        }

        public async Task<HallRequestDto> Handle(HallGetByIdRequest request, CancellationToken cancellationToken)
        {
            var hall = await hallRepository.GetAll()
                .Include(h => h.Seats)
                .ThenInclude(s => s.SeatReservations)
                .FirstOrDefaultAsync(h => h.Id == request.HallId, cancellationToken);

            if (hall == null)
            {
                return null;
            }

            return new HallRequestDto
            {
                Id = hall.Id,
                Name = hall.Name,
                Rows = hall.Rows,
                SeatsPerRow = hall.SeatsPerRow, // Assuming SeatsPerRow is already a List<int>
                Seats = hall.Seats.Select(seat => new SeatDto
                {
                    //Id = seat.Id,
                    Row = seat.Row,
                    Number = seat.Number,
                    Price = seat.Price,
                    IsReserved = seat.SeatReservations.Any()
                }).ToList()
            };
        }
    }
}
