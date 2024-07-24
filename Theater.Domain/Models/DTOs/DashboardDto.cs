using System.Collections.Generic;

namespace Theater.Domain.Models.DTOs
{
    public class DashboardResponseDto
    {
        public ShowDateDto UpcomingShowDate { get; set; }
        public List<ShowDateDto> BestShowDates { get; set; }
        public List<ShowDateDto> WorstShowDates { get; set; }
        public List<RoleUsersDto> UsersWithRoles { get; set; }
        public  int UsersWithoutRoles { get; set; }
        public List<string> UsersWithoutRolesList { get; set; }
        public List<MonthlyRevenueDto> MonthlyRevenue { get; set; }
        public List<MonthlyTicketsSoldDto> MonthlyTicketsSold { get; set; }

    }

    public class ShowDateDto
    {
        public string PosterTitle { get; set; }
        public DateTime Date { get; set; }
        public int SoldTickets { get; set; }
        public int UnsoldTickets { get; set; }
        public decimal Revenue { get; set; }
    }

    public class RoleUsersDto
    {
        public string Role { get; set; }
        public List<string> Users { get; set; }
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
