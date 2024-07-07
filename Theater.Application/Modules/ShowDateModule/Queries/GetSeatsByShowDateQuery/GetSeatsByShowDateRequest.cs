using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Theater.Application.Modules.PosterModule.Queries.PosterBuyTicketQuery;

namespace Theater.Application.Modules.ShowDateModule.Queries.GetSeatsByShowDateQuery
{
    public class GetSeatsByShowDateRequest : IRequest<IEnumerable<SeatDto>>
    {
        public int ShowDateId { get; set; }
    }
}

