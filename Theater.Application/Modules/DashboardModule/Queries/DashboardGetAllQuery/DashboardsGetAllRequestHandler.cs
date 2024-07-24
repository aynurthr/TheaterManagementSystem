using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Theater.Application.Repositories;
using Theater.Domain.Models.DTOs;
using Theater.Domain.Models.Entities.Membership;

namespace Theater.Application.Modules.DashboardModule.Queries.DashboardGetAllQuery
{
    public class DashboardsGetAllRequestHandler : IRequestHandler<DashboardGetAllRequest, DashboardResponseDto>
    {
        private readonly IShowDateRepository _showDateRepository;
        private readonly ITicketRepository _ticketRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IPosterRepository _posterRepository;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IActionContextAccessor _ctx;

        public DashboardsGetAllRequestHandler(
            IShowDateRepository showDateRepository,
            ITicketRepository ticketRepository,
            IUserRepository userRepository,
            IUserRoleRepository userRoleRepository,
            IRoleRepository roleRepository,
            IPosterRepository posterRepository,
            UserManager<AppUser> userManager,
            RoleManager<AppRole> roleManager,
            IActionContextAccessor ctx)
        {
            _showDateRepository = showDateRepository;
            _ticketRepository = ticketRepository;
            _userRepository = userRepository;
            _userRoleRepository = userRoleRepository;
            _roleRepository = roleRepository;
            _posterRepository = posterRepository;
            _userManager = userManager;
            _roleManager = roleManager;
            _ctx = ctx;
        }

        public async Task<DashboardResponseDto> Handle(DashboardGetAllRequest request, CancellationToken cancellationToken)
        {
            var upcomingShowDate = await _showDateRepository.GetAll()
                .Where(sd => sd.Date > DateTime.Now && sd.DeletedAt == null)
                .OrderBy(sd => sd.Date)
                .FirstOrDefaultAsync();

            if (upcomingShowDate == null)
            {
                return new DashboardResponseDto();
            }

            var upcomingShowDateTickets = await _ticketRepository.GetAll()
                .Where(t => t.ShowDateId == upcomingShowDate.Id && t.DeletedAt == null)
                .ToListAsync();

            var soldTickets = upcomingShowDateTickets.Count(t => t.IsPurchased);
            var unsoldTickets = upcomingShowDateTickets.Count - soldTickets;
            var revenue = upcomingShowDateTickets.Where(t => t.IsPurchased).Sum(t => t.Price);

            var upcomingPoster = await _posterRepository.GetAsync(p => p.Id == upcomingShowDate.PosterId && p.DeletedAt == null);

            var showDates = await _showDateRepository.GetAll()
                .Where(sd => sd.DeletedAt == null)
                .Include(sd => sd.Tickets.Where(t => t.DeletedAt == null))
                .Include(sd => sd.Poster)
                .ToListAsync();

            var bestShowDates = showDates
                .OrderByDescending(sd => sd.Tickets.Where(t => t.IsPurchased).Sum(t => t.Price))
                .Take(3)
                .ToList();

            var worstShowDates = showDates
                .OrderByDescending(sd => sd.Tickets.Where(t => !t.IsPurchased).Sum(t => t.Price))
                .Take(3)
                .ToList();

            var usersWithRoles = await _userRoleRepository.GetAll().ToListAsync();
            var userIds = usersWithRoles.Select(ur => ur.UserId).Distinct().ToList();
            var roleIds = usersWithRoles.Select(ur => ur.RoleId).Distinct().ToList();

            var users = await _userManager.Users.Where(u => userIds.Contains(u.Id)).ToListAsync();
            var roles = await _roleManager.Roles.Where(r => roleIds.Contains(r.Id)).ToListAsync();

            var userRoleDetails = usersWithRoles.Select(ur => new
            {
                User = users.FirstOrDefault(u => u.Id == ur.UserId),
                Role = roles.FirstOrDefault(r => r.Id == ur.RoleId)
            }).Where(ur => ur.User != null && ur.Role != null).ToList();

            var usersWithoutRolesList = await _userManager.Users
                .Where(u => !userRoleDetails.Select(ur => ur.User.Id).Contains(u.Id))
                .Select(u => u.UserName)
                .ToListAsync();

            var usersCount = await _userManager.Users.CountAsync();
            var usersWithoutRolesCount = usersCount - userRoleDetails.Select(ur => ur.User.Id).Distinct().Count();

            var now = DateTime.Now;
            var lastSixMonths = Enumerable.Range(0, 6)
                .Select(i => new
                {
                    Year = now.AddMonths(-i).Year,
                    Month = now.AddMonths(-i).Month
                })
                .OrderBy(x => new DateTime(x.Year, x.Month, 1))
                .ToList();

            var revenueData = await _ticketRepository.GetAll()
                .Where(t => t.IsPurchased && t.DeletedAt == null)
                .ToListAsync();

            var monthlyRevenue = lastSixMonths.GroupJoin(
                revenueData,
                date => new { date.Year, date.Month },
                ticket => new { Year = ticket.IsPurchasedAt.Value.Year, Month = ticket.IsPurchasedAt.Value.Month },
                (date, tickets) => new MonthlyRevenueDto
                {
                    Year = date.Year,
                    Month = date.Month,
                    Revenue = tickets.Sum(t => t.Price)
                }
            ).ToList();


            return new DashboardResponseDto
            {
                UpcomingShowDate = new ShowDateDto
                {
                    PosterTitle = upcomingPoster?.Title,
                    Date = upcomingShowDate.Date,
                    SoldTickets = soldTickets,
                    UnsoldTickets = unsoldTickets,
                    Revenue = revenue
                },
                BestShowDates = bestShowDates.Select(sd => new ShowDateDto
                {
                    PosterTitle = sd.Poster.Title,
                    Date = sd.Date,
                    SoldTickets = sd.Tickets.Count(t => t.IsPurchased),
                    UnsoldTickets = sd.Tickets.Count(t => !t.IsPurchased),
                    Revenue = sd.Tickets.Where(t => t.IsPurchased).Sum(t => t.Price)
                }).ToList(),
                WorstShowDates = worstShowDates.Select(sd => new ShowDateDto
                {
                    PosterTitle = sd.Poster.Title,
                    Date = sd.Date,
                    SoldTickets = sd.Tickets.Count(t => t.IsPurchased),
                    UnsoldTickets = sd.Tickets.Count(t => !t.IsPurchased),
                    Revenue = sd.Tickets.Where(t => !t.IsPurchased).Sum(t => t.Price)
                }).ToList(),
                UsersWithRoles = userRoleDetails.GroupBy(ur => ur.Role.Name)
                    .Select(g => new RoleUsersDto
                    {
                        Role = g.Key,
                        Users = g.Select(ur => ur.User.UserName).ToList()
                    }).ToList(),
                UsersWithoutRoles = usersWithoutRolesCount,
                UsersWithoutRolesList = usersWithoutRolesList,
                MonthlyRevenue = monthlyRevenue
            };
        }
    }
}
