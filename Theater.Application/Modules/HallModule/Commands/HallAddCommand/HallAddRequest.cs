using MediatR;
using Theater.Application.Modules.HallModule.Queries;

namespace Theater.Application.Modules.HallModule.Commands.HallAddCommand
{
    public class HallAddRequest : IRequest<HallRequestDto>
    {
        public string Name { get; set; }
        public int Capacity { get; set; }
    }
}
