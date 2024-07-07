using MediatR;
using Microsoft.EntityFrameworkCore;
using Theater.Application.Repositories;

namespace Theater.Application.Modules.HallModule.Queries.HallGetAllQuery
{
    public class HallGetAllRequestHandler : IRequestHandler<HallGetAllRequest, IEnumerable<HallRequestDto>>
    {
        private readonly IHallRepository _hallRepository;

        public HallGetAllRequestHandler(IHallRepository hallRepository)
        {
            _hallRepository = hallRepository;
        }

        public async Task<IEnumerable<HallRequestDto>> Handle(HallGetAllRequest request, CancellationToken cancellationToken)
        {
            var query = _hallRepository.GetAll();

            if (request.OnlyAvailable)
            {
                query = query.Where(m => m.DeletedAt == null);
            }

            var queryResponse = await query
                .OrderBy(m => m.Name)
                .Select(m => new HallRequestDto
                {
                    Id = m.Id,
                    Name = m.Name,
                    Capacity=m.Capacity
                }).ToListAsync(cancellationToken);

            return queryResponse;
        }
    }
}
