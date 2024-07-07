using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Theater.Application.Repositories;
using Theater.Application.Modules.SeatModule.Queries;

namespace Theater.Application.Modules.HallModule.Queries.HallGetByIdQuery
{
    public class HallGetByIdRequestHandler : IRequestHandler<HallGetByIdRequest, HallRequestDto>
    {
        private readonly IHallRepository _hallRepository;
        private readonly ISeatRepository _seatRepository;

        public HallGetByIdRequestHandler(IHallRepository hallRepository, ISeatRepository seatRepository)
        {
            _hallRepository = hallRepository;
            _seatRepository = seatRepository;
        }

        public async Task<HallRequestDto> Handle(HallGetByIdRequest request, CancellationToken cancellationToken)
        {
            var hall = await _hallRepository.GetAsync(h => h.Id == request.Id);
            if (hall == null)
            {
                return null;
            }

            var seats = await _seatRepository.GetAll(s => s.HallId == request.Id && s.DeletedAt == null).ToListAsync(cancellationToken);

            var hallDto = new HallRequestDto
            {
                Id = hall.Id,
                Name = hall.Name,
                Capacity = hall.Capacity,
                Seats = seats.Select(s => new SeatRequestDto
                {
                    Row = s.Row,
                    Number = s.Number
                }).ToList()
            };

            return hallDto;
        }
    }
}
