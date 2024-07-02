using Theater.Domain.Models.Entities.Membership;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Theater.Application.Modules.AccountModule.Commands.PrincipalFillCommand
{
    class PrincipalFillRequestHandler : IRequestHandler<PrincipalFillRequest>
    {
        private readonly DbContext db;

        public PrincipalFillRequestHandler(DbContext db)
        {
            this.db = db;
        }
        public async Task Handle(PrincipalFillRequest request, CancellationToken cancellationToken)
        {
            var userId = Convert.ToInt32(request.Identity.Claims.FirstOrDefault(m => m.Type.Equals(ClaimTypes.NameIdentifier))?.Value);

            var roles = await (from ur in db.Set<AppUserRole>()
                         join r in db.Set<AppRole>() on ur.RoleId equals r.Id
                         where ur.UserId == userId
                         select r.NormalizedName)
                         .Distinct()
                         .ToListAsync(cancellationToken);


            foreach (var role in roles)
            {
                request.Identity.AddClaim(new Claim(ClaimTypes.Role, role));
            }

            var claims = await db.Set<AppUserClaim>().Where(m => m.UserId == userId && "1".Equals(m.ClaimValue))
                .Select(m => m.ClaimType)
                .Union((from ur in db.Set<AppUserRole>()
                        join rc in db.Set<AppRoleClaim>() on ur.RoleId equals rc.RoleId
                        where ur.UserId == userId && rc.ClaimValue == "1"
                        select rc.ClaimType).Distinct())
                .ToListAsync(cancellationToken);

            foreach (var claim in claims)
            {
                request.Identity.AddClaim(new Claim(claim, "1"));
            }
        }
    }
}
