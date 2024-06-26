using MediatR;

namespace Theater.Application.Modules.HallModule.Queries
{
    public class HallGetByIdRequest : IRequest<HallRequestDto>
    {
        public int HallId { get; set; }
    }
}
