using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Theater.Application.Modules.ShowDateModule.Queries
{
    public class TicketDto
    {
        public int ShowDateId { get; set; }
        public int SeatId { get; set; }
        public decimal Price { get; set; }
    }
}
