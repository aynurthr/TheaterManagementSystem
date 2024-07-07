namespace Theater.Application.Modules.SeatModule.Queries
{
    public class SeatRequestDto
    {
        public int HallId { get; set; }
        public string Row { get; set; }
        public int Number { get; set; }
    }
}