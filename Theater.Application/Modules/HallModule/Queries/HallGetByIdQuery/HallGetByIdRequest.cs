using MediatR;
using Theater.Domain.Models.Entities;

namespace Theater.Application.Modules.HallModule.Queries.HallGetByIdQuery
{
    public class HallGetByIdRequest : IRequest<HallRequestDto>
    {
        public int Id { get; set; }
    }
}
