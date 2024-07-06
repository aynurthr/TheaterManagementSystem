using MediatR;

namespace Theater.Application.Modules.PosterModule.Queries.PosterBuyTicketQuery
{
    public class PosterBuyTicketRequest : IRequest<PosterBuyTicketResponseDto>
    {
        public int PosterId { get; set; }
        public int ShowDateId { get; set; }
    }
}

