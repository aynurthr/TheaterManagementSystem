using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Theater.DataAccessLayer.Contexts;
using Theater.Domain.Models.DTOs;
using Theater.Domain.Models.Entities.Membership;

namespace Theater.Presentation.Controllers
{
    [Area("Admin")]
    public class UsersController : Controller
    {
        private readonly UserManager<AppUser> userManager;
        private readonly DataContext db;

        public UsersController(UserManager<AppUser> userManager, DataContext db)
        {
            this.userManager = userManager;
            this.db = db;
        }

        public async Task<IActionResult> Index()
        {
            var users = await userManager.Users.ToListAsync();
            return View(users);
        }

        public async Task<IActionResult> Details(string id)
        {
            var user = await userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            AppUserDto dto = new AppUserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                EmailConfirmed = user.EmailConfirmed,
                Phone = user.PhoneNumber,
                PhoneConfirmed = user.PhoneNumberConfirmed
            };

            try
            {
                var userClaims = await userManager.GetClaimsAsync(user);
                if (userClaims == null || !userClaims.Any())
                {
                    dto.Policies = new List<PolicyDto>(); // No policies found, set an empty list
                }
                else
                {
                    var userPolicies = userClaims.Where(m => m.Value == "1").Select(m => m.Type);
                    dto.Policies = (from p in Program.policies
                                    join up in userPolicies on p equals up into leftSet
                                    from ls in leftSet.DefaultIfEmpty()
                                    select new PolicyDto
                                    {
                                        Name = p,
                                        Selected = ls != null
                                    }).ToList();
                }

                dto.Roles = await (from r in db.Roles
                                   join ur in db.UserRoles.Where(m => m.UserId == user.Id) on r.Id equals ur.RoleId into leftSet
                                   from ls in leftSet.DefaultIfEmpty()
                                   select new AppUserRoleDto
                                   {
                                       Id = r.Id,
                                       Name = r.Name,
                                       Selected = ls != null
                                   }).ToListAsync();

                return View(dto);
            }
            catch
            {
                // If any exception occurs, return a 500 status code with a generic error message
                return StatusCode(500, "Internal server error");
            }

        }
    }
}
