using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Theater.Domain.Models.Entities.Membership;

namespace Theater.Application.Modules.MembershipRolesModule
{
    public class AppRoleValidator : AbstractValidator<AppRole>
    {
        private readonly RoleManager<AppRole> _roleManager;

        public AppRoleValidator(RoleManager<AppRole> roleManager)
        {
            _roleManager = roleManager;

            RuleFor(role => role.Name)
                .NotEmpty().WithMessage("Role name is required.")
                .Must(BeUniqueName).WithMessage("Role name already exists.");
        }

        private bool BeUniqueName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return true; // If the name is empty or null, the NotEmpty rule will handle it.
            }

            var existingRole = _roleManager.FindByNameAsync(name).Result;
            return existingRole == null;
        }
    }
}
