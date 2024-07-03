using Theater.Domain.Models.Entities.Membership;
using Theater.Infrastructure.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Theater.Application.Modules.RolesModule.Commands.ManageMemberCommand
{
    class ManageMemberRequestHandler : IRequestHandler<ManageMemberRequest>
    {
        private readonly DbContext db;
        private readonly IActionContextAccessor ctx;

        public ManageMemberRequestHandler(DbContext db, IActionContextAccessor ctx)
        {
            this.db = db;
            this.ctx = ctx;
        }

        public async Task Handle(ManageMemberRequest request, CancellationToken cancellationToken)
        {
            var userId = Convert.ToInt32(ctx.ActionContext.HttpContext.User.Claims.FirstOrDefault(m => m.Type.Equals(ClaimTypes.NameIdentifier))?.Value);

            if (request.MemberId == userId)
            {
                throw new BadRequestException("istifadeci ozune aid huquqlari deyise bilmez");
            }


            var table = db.Set<AppUserRole>();

            AppUserRole userRole = default;

            if (request.Selected)
            {
                userRole = await table.FirstOrDefaultAsync(m => m.UserId == request.MemberId && m.RoleId == request.RoleId, cancellationToken);

                if (userRole is not null)
                {
                    throw new BadRequestException("bu istifadeci onsuz da hemin role-dadir");
                }

                userRole = new AppUserRole
                {
                    UserId = request.MemberId,
                    RoleId = request.RoleId
                };

                await table.AddAsync(userRole, cancellationToken);
                await db.SaveChangesAsync(cancellationToken);

            }
            else
            {
                userRole = await table.FirstOrDefaultAsync(m => m.UserId == request.MemberId && m.RoleId == request.RoleId, cancellationToken);

                if (userRole is null)
                    throw new BadRequestException("bu istifadeci bu rol-da deyil");

                table.Remove(userRole);
                await db.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
