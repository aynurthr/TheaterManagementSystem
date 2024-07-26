namespace Theater.Domain.Models.DTOs
{
    public class DashboardResponseDto
    {
        public ShowDateDto UpcomingShowDate { get; set; }
        public List<ShowDateDto> BestShowDates { get; set; }
        public List<ShowDateDto> WorstShowDates { get; set; }
        public List<UserRoleDto> UsersWithRoles { get; set; } // Unified user list
        public List<MonthlyRevenueDto> MonthlyRevenue { get; set; }
        public List<MonthlyTicketsSoldDto> MonthlyTicketsSold { get; set; }
    }

    public class UserTicketDataDto
    {
        public int? UserId { get; set; }
        public string UserName { get; set; }
        public int TicketsBought { get; set; }
        public decimal Revenue { get; set; }
    }

    public class ShowDateDto
    {
        public string PosterTitle { get; set; }
        public DateTime Date { get; set; }
        public int SoldTickets { get; set; }
        public int UnsoldTickets { get; set; }
        public decimal Revenue { get; set; }
    }

    public class UserRoleDto
    {
        public string UserName { get; set; }
        public string? Role { get; set; } // Can be null if no role
        public int TicketsBought { get; set; }
        public decimal Revenue { get; set; }
    }

    public class MonthlyRevenueDto
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public decimal Revenue { get; set; }
    }

    public class MonthlyTicketsSoldDto
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public int TicketsSold { get; set; }
    }
}
