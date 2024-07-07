using MediatR;

namespace Theater.Application.Modules.ShowDateModule.Queries.GetTicketBySeatAndShowDateQuery
{
    public class GetTicketBySeatAndShowDateRequest : IRequest<TicketDto>
    {
        public int ShowDateId { get; set; }
        public int SeatId { get; set; }
    }
}
