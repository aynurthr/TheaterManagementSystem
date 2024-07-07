namespace Theater.Application.Modules.PosterModule.Queries.PosterBuyTicketQuery
{
    public class PosterBuyTicketResponseDto
    {
        public int PosterId { get; set; }
        public int ShowDateId { get; set; }
        public DateTime Date { get; set; }
        public List<SeatDto> Seats { get; set; }
        public string Title { get; set; }
        public string ImageSrc { get; set; }
        public List<ShowDateDto> ShowDates { get; set; }
    }

    public class SeatDto
    {
        public int Id { get; set; } // Add this line
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
