using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Theater.Domain.Models.Entities.Membership;
using Theater.Infrastructure.Abstracts;
using Theater.Presentation.Models;

namespace Theater.Presentation.Controllers
{
    [Authorize]
    public class UserProfileController : Controller
    {
        private readonly UserManager<AppUser> userManager;
        private readonly IIdentityService identityService;

        public UserProfileController(UserManager<AppUser> userManager, IIdentityService identityService)
        {
            this.userManager = userManager;
            this.identityService = identityService;
        }

        [HttpGet]
        [Route("/profile.html")]
        public async Task<IActionResult> Profile()
        {
            var userId = identityService.GetPrincipialId();
            if (userId == null)
            {
                return NotFound("User ID not found.");
            }

            var user = await userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                return NotFound("User not found.");
            }

            var model = new UserProfileViewModel
            {
                UserName = user.UserName,
                Name = user.Name,
                Surname = user.Surname,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber
            };

            if (TempData["EditSuccess"] != null)
            {
                ViewBag.EditSuccess = TempData["EditSuccess"];
            }

            return View(model);
        }

        [HttpGet]
        [Route("/profile/edit.html")]
        public async Task<IActionResult> EditProfile()
        {
            var userId = identityService.GetPrincipialId();
            if (userId == null)
            {
                return NotFound("User ID not found.");
            }

            var user = await userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                return NotFound("User not found.");
            }

            var model = new UserProfileViewModel
            {
                UserName = user.UserName,
                Name = user.Name,
                Surname = user.Surname,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber
            };

            return View(model);
        }

        [HttpPost]
        [Route("/profile/edit.html")]
        public async Task<IActionResult> EditProfile(UserProfileViewModel model)
        {
            var userId = identityService.GetPrincipialId();
            var user = await userManager.FindByIdAsync(userId.ToString());

            if (user == null)
            {
                return NotFound("User not found.");
            }

            // Check if email or phone number is already in use
            var emailInUse = await userManager.Users.AnyAsync(u => u.Email == model.Email && u.Id != user.Id);
            var phoneNumberInUse = await userManager.Users.AnyAsync(u => u.PhoneNumber == model.PhoneNumber && u.Id != user.Id);

            if (emailInUse)
            {
                ModelState.AddModelError("Email", "Email is already in use.");
                return View(model);
            }

            if (phoneNumberInUse)
            {
                ModelState.AddModelError("PhoneNumber", "Phone number is already in use.");
                return View(model);
            }

            if (!string.IsNullOrEmpty(model.NewPassword))
            {
                if (string.IsNullOrEmpty(model.CurrentPassword) || model.NewPassword != model.ConfirmNewPassword)
                {
                    ModelState.AddModelError(string.Empty, "Current password is required and new passwords must match.");
                    return View(model);
                }

                var isCurrentPasswordValid = await userManager.CheckPasswordAsync(user, model.CurrentPassword);
                if (!isCurrentPasswordValid)
                {
                    ModelState.AddModelError(string.Empty, "The current password is incorrect.");
                    return View(model);
                }

                var changePasswordResult = await userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
                if (!changePasswordResult.Succeeded)
                {
                    foreach (var error in changePasswordResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return View(model);
                }
            }

            // Update user details only if current password validation passes or no password change is requested
            user.Name = model.Name;
            user.Surname = model.Surname;
            user.Email = model.Email;
            user.PhoneNumber = model.PhoneNumber;

            var updateResult = await userManager.UpdateAsync(user);

            if (!updateResult.Succeeded)
            {
                foreach (var error in updateResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(model);
            }

            TempData["EditSuccess"] = "Your profile has been updated successfully.";
            return RedirectToAction("Profile");
        }
    }
}
