using System;
using System.Collections.Generic;

namespace Theater.Application.Modules.PosterModule.Queries.PosterBuyTicketQuery
{
    public class PosterBuyTicketResponseDto
    {
        public int ShowDateId { get; set; }
        public DateTime Date { get; set; }
        public List<SeatDto> Seats { get; set; }
        public string Title { get; set; } // Add this
        public string ImageSrc { get; set; } // Add this
        public List<ShowDateDto> ShowDates { get; set; } // Add this
    }

    public class SeatDto
    {
        public string Row { get; set; }
        public int SeatNumber { get; set; }
        public bool IsPurchased { get; set; }
        public decimal Price { get; set; }
    }

    public class ShowDateDto
    {
        public int ShowDateId { get; set; }
        public DateTime Date { get; set; }
    }
}
