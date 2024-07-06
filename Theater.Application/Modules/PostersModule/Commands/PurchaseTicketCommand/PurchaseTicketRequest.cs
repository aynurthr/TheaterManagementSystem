using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Theater.Application.Modules.PostersModule.Commands.PurchaseTicketCommand
{
    public class PurchaseTicketRequest : IRequest<bool>
    {
        public int UserId { get; set; }
        public List<int> SeatIds { get; set; }
    }
}

