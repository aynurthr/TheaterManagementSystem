using MediatR;
using Theater.Application.Modules.SeatModule.Queries;

namespace Theater.Application.Modules.SeatModule.Commands.AddRemoveSeatsCommand
{
    public class AddRemoveSeatsRequest : IRequest<bool>
    {
        public int HallId { get; set; }
        public List<SeatRequestDto> SeatsToAdd { get; set; }
        public List<SeatRequestDto> SeatsToRemove { get; set; }
    }

}
