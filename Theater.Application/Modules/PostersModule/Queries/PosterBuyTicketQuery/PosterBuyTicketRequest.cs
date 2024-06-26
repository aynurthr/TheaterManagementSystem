using MediatR;

namespace Theater.Application.Modules.PosterModule.Queries.PosterBuyTicketRequestDto
{
    public class PosterBuyTicketRequest : IRequest<SeatingChartViewModel>
    {
        public int PosterId { get; set; }
        public int ShowDateId { get; set; }
    }
}
