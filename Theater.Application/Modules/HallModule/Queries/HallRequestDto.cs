using Theater.Application.Modules.PosterModule.Queries.PosterBuyTicketQuery;
using Theater.Application.Modules.SeatModule.Queries;

namespace Theater.Application.Modules.HallModule.Queries
{
    public class HallRequestDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Capacity { get; set; }
        public List<SeatRequestDto> Seats { get; set; }
    }
}


