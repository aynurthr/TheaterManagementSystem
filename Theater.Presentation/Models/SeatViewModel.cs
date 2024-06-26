namespace Theater.Presentation.Models
{
    public class SeatingChartViewModel
    {
        public int ShowDateId { get; set; }
        public List<SeatDto> Seats { get; set; }
        public List<ShowDateDto> ShowDates { get; set; }
        public string Title { get; set; }
    }

    public class SeatDto
    {
        public int Row { get; set; }
        public int Number { get; set; }
        public decimal Price { get; set; }
        public bool IsReserved { get; set; }
    }

    public class ShowDateDto
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
    }
}
