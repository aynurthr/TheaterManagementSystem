using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Theater.DataAccessLayer.Contexts;
using Theater.Domain.Models.DTOs;
using Theater.Domain.Models.Entities.Membership;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Theater.Presentation.Pipeline;
using Microsoft.AspNetCore.Authorization;

namespace Theater.Presentation.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize("users.manage")]
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
                Surname = user.Surname,
                Name = user.Name,
                Email = user.Email,
                EmailConfirmed = user.EmailConfirmed,
                Phone = user.PhoneNumber,
                PhoneConfirmed = user.PhoneNumberConfirmed,
                Policies = new List<PolicyDto>(),
                Roles = new List<AppUserRoleDto>()
            };

            try
            {
                // Get user roles
                var userRoles = await (from r in db.Roles
                                       join ur in db.UserRoles on r.Id equals ur.RoleId
                                       where ur.UserId == user.Id
                                       select new AppUserRoleDto
                                       {
                                           Id = r.Id,
                                           Name = r.Name,
                                           Selected = true
                                       }).ToListAsync();

                dto.Roles = userRoles;

                // Get all available claims
                var allClaims = AppClaimsTransformation.policies;

                // Get claims for user roles
                var roleIds = userRoles.Select(r => r.Id).ToList();
                var roleClaims = await (from rc in db.RoleClaims
                                        where roleIds.Contains(rc.RoleId)
                                        select new PolicyDto
                                        {
                                            Name = rc.ClaimType,
                                            Selected = rc.ClaimValue == "1"
                                        }).ToListAsync();

                // Mark claims based on user roles
                dto.Policies = allClaims.Select(claim => new PolicyDto
                {
                    Name = claim,
                    Selected = roleClaims.Any(rc => rc.Name == claim && rc.Selected) ||
                               userRoles.Any(ur => ur.Name.ToLower() == "superadmin")
                }).ToList();

                return View(dto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
