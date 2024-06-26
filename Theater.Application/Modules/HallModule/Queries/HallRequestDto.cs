using System.Collections.Generic;
using Theater.Application.Modules.PosterModule.Queries.PosterBuyTicketRequestDto;

public class HallRequestDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Rows { get; set; }
    public List<int> SeatsPerRow { get; set; }
    public List<SeatDto> Seats { get; set; } // Add this property
}
