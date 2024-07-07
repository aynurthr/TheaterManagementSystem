using MediatR;
using Theater.Application.Repositories;
using Theater.Domain.Models.Entities;
using System.Threading;
using System.Threading.Tasks;
using Theater.Application.Modules.HallModule.Queries;

namespace Theater.Application.Modules.HallModule.Commands.HallAddCommand
{
    public class HallAddRequestHandler : IRequestHandler<HallAddRequest, HallRequestDto>
    {
        private readonly IHallRepository _hallRepository;

        public HallAddRequestHandler(IHallRepository hallRepository)
        {
            _hallRepository = hallRepository;
        }

        public async Task<HallRequestDto> Handle(HallAddRequest request, CancellationToken cancellationToken)
        {
            var entity = new Hall
            {
                Name = request.Name,
                Capacity = request.Capacity
            };

            await _hallRepository.AddAsync(entity, cancellationToken);
            await _hallRepository.SaveAsync(cancellationToken);

            var dto = new HallRequestDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Capacity = entity.Capacity
            };

            return dto;
        }
    }
}
