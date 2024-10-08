﻿using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Theater.DataAccessLayer.Contexts;
using Theater.Domain.Models.DTOs;
using Theater.Domain.Models.Entities.Membership;
using Theater.Presentation.Pipeline;

namespace Theater.Presentation.Controllers
{
    [Area("Admin")]

    public class RolesController : Controller
    {
        private readonly RoleManager<AppRole> roleManager;
        private readonly IValidator<AppRole> roleValidator;
        private readonly DataContext db;

        public RolesController(RoleManager<AppRole> roleManager, DataContext db, IValidator<AppRole> roleValidator)
        {
            this.roleManager = roleManager;
            this.db = db;
            this.roleValidator = roleValidator;
        }

        [Authorize("roles.index")]
        public async Task<IActionResult> Index()
        {
            var roles = await roleManager.Roles.ToListAsync();
            return View(roles);
        }

        [Authorize("roles.details")]
        public async Task<IActionResult> Details(int id)
        {
            AppRole role = await roleManager.FindByIdAsync(id.ToString());

            if (role == null)
            {
                return NotFound();
            }

            AppRoleDto dto = new AppRoleDto
            {
                Id = role.Id,
                Name = role.Name
            };

            try
            {
                var roleClaims = await roleManager.GetClaimsAsync(role);
                var rolePolicies = roleClaims.Where(m => m.Value == "1").Select(m => m.Type).ToList();

                if (role.Name.ToLower() == "superadmin")
                {
                    // If the role is superadmin, assign all policies as selected
                    dto.Policies = AppClaimsTransformation.policies.Select(p => new PolicyDto
                    {
                        Name = p,
                        Selected = true // All policies selected for superadmin
                    }).ToList();
                }
                else
                {
                    // Regular roles
                    dto.Policies = AppClaimsTransformation.policies.Select(p => new PolicyDto
                    {
                        Name = p,
                        Selected = rolePolicies.Contains(p)
                    }).ToList();
                }

                dto.Members = await (from u in db.Users
                                     join ur in db.UserRoles on u.Id equals ur.UserId
                                     where ur.RoleId == role.Id
                                     select new AppRoleMemberDto
                                     {
                                         Id = u.Id,
                                         Name = $"{u.UserName} ({u.Email})",
                                         Selected = true
                                     }).ToListAsync();

                return View(dto);
            }
            catch (Exception ex)
            {
                // Detailed error message for debugging
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }



        [Authorize("roles.create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize("roles.create")]
        public async Task<IActionResult> Create(AppRole role)
        {
            var validationResult = roleValidator.Validate(role); // Synchronous validation

            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.ErrorMessage);
                }
                return View(role);
            }

            if (ModelState.IsValid)
            {
                try
                {
                    role.ConcurrencyStamp = Guid.NewGuid().ToString(); // Set ConcurrencyStamp

                    var result = await roleManager.CreateAsync(role);

                    if (result.Succeeded)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
                catch (DbUpdateException dbEx)
                {
                    var innerException = dbEx.InnerException?.Message ?? dbEx.Message;
                    return StatusCode(500, $"Database error: {innerException}");
                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"Exception: {ex.Message}");
                }
            }
            return View(role);
        }

        [Authorize("roles.edit")]
        public async Task<IActionResult> Edit(int id)
        {
            AppRole role = await roleManager.FindByIdAsync(id.ToString());

            if (role is null)
            {
                return NotFound();
            }

            AppRoleDto dto = new AppRoleDto
            {
                Id = role.Id,
                Name = role.Name
            };


            #region Policies
            var rolePolicies = (await roleManager.GetClaimsAsync(role)).Where(m => m.Value == "1").Select(m => m.Type);


            dto.Policies = (from p in AppClaimsTransformation.policies
                            join rp in rolePolicies on p equals rp into leftSet
                            from ls in leftSet.DefaultIfEmpty()
                            select new PolicyDto
                            {
                                Name = p,
                                Selected = ls != null
                            });
            #endregion

            #region Members
            dto.Members = await (from u in db.Users
                                 join ur in db.UserRoles.Where(m => m.RoleId == role.Id) on u.Id equals ur.UserId into leftSet
                                 from ls in leftSet.DefaultIfEmpty()
                                 where !db.UserRoles.Any(sur => sur.UserId == u.Id && db.Roles.Any(r => r.Id == sur.RoleId && r.Name.ToLower() == "superadmin"))
                                 select new AppRoleMemberDto
                                 {
                                     Id = u.Id,
                                     Name = $"{u.UserName} ({u.Email})",
                                     Selected = ls != null
                                 }).ToListAsync();
            #endregion

            return View(dto);
        }

        [HttpPost]
        [Authorize("roles.edit")]
        public async Task<IActionResult> Edit(AppRole role)
        {
            AppRole entity = await roleManager.FindByIdAsync(role.Id.ToString());
            entity.Name = role.Name;
            await roleManager.UpdateAsync(entity);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [Authorize("roles.remove")]
        public async Task<IActionResult> Delete(int id)
        {
            AppRole role = await roleManager.FindByIdAsync(id.ToString());
            if (role == null)
            {
                return NotFound();
            }

            // Remove related entries in AppUserRoles
            var userRoles = await db.UserRoles.Where(ur => ur.RoleId == id).ToListAsync();
            db.UserRoles.RemoveRange(userRoles);

            // Remove related entries in AppRoleClaims
            var roleClaims = await db.RoleClaims.Where(rc => rc.RoleId == id).ToListAsync();
            db.RoleClaims.RemoveRange(roleClaims);

            await db.SaveChangesAsync();

            var result = await roleManager.DeleteAsync(role);
            if (!result.Succeeded)
            {
                return BadRequest("Failed to delete the role.");
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [Authorize("roles.change-access")]
        public async Task<IActionResult> ChangeAccess(string policyName, int roleId, bool selected)
        {
            var table = db.Set<AppRoleClaim>();
            AppRoleClaim claim = default;
            if (selected)
            {
                claim = await table.FirstOrDefaultAsync(m => m.RoleId == roleId && m.ClaimType == policyName);

                if (claim is not null && claim.ClaimValue == "1")
                {
                    return Json(new { error = true, message = "artiq icaze verilib" });
                }
                else if (claim is not null)
                {
                    claim.ClaimValue = "1";
                    await db.SaveChangesAsync();
                    goto l1;
                }
                else
                {
                    claim = new AppRoleClaim
                    {
                        RoleId = roleId,
                        ClaimType = policyName,
                        ClaimValue = "1"
                    };
                    await table.AddAsync(claim);
                    await db.SaveChangesAsync();
                    goto l1;
                }
            }
            else
            {
                claim = await table.FirstOrDefaultAsync(m => m.RoleId == roleId && m.ClaimType == policyName);

                if (claim is null)
                {
                    return Json(new { error = true, message = "icaze movcud deyil" });
                }
                else
                {
                    table.Remove(claim);
                    await db.SaveChangesAsync();
                    goto l1;
                }
            }



        l1:
            return Json(new { error = false, message = "icra edildi" });
        }

        [HttpPost]
        [Authorize("roles.manage-members")]
        public async Task<IActionResult> ManageMember(int memberId, int roleId, bool selected)
        {
            var userId = Convert.ToInt32(User.Claims.FirstOrDefault(m => m.Type.Equals(ClaimTypes.NameIdentifier))?.Value);

            if (memberId == userId)
            {
                return Json(new { error = true, message = "istifadeci ozune aid huquqlari deyise bilmez" });
            }


            var table = db.Set<AppUserRole>();

            AppUserRole userRole = default;

            if (selected)
            {
                userRole = await table.FirstOrDefaultAsync(m => m.UserId == memberId && m.RoleId == roleId);

                if (userRole is not null)
                {
                    return Json(new { error = true, message = "bu istifadeci onsuz da hemin role-dadir" });
                }

                userRole = new AppUserRole
                {
                    UserId = memberId,
                    RoleId = roleId
                };

                await table.AddAsync(userRole);
                await db.SaveChangesAsync();

            }
            else
            {
                userRole = await table.FirstOrDefaultAsync(m => m.UserId == memberId && m.RoleId == roleId);

                if (userRole is null)
                {
                    return Json(new { error = true, message = "bu istifadeci bu rol-da deyil" });
                }

                table.Remove(userRole);
                await db.SaveChangesAsync();
            }

        l1:
            return Json(new { error = false, message = "icra edildi" });
        }
    }
}
