using MediatR;
using System.Collections.Generic;

namespace Theater.Application.Modules.PosterModule.Commands.PurchaseTicketCommand
{
    public class PurchaseTicketRequest : IRequest<bool>
    {
        public int UserId { get; set; }
        public List<int> SeatIds { get; set; }
        public int ShowDateId { get; set; }
    }
}
