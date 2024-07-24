using Theater.Domain.Models.Entities;
using Theater.Domain.Models.Entities.Membership;
using Theater.Infrastructure.Abstracts;

namespace Theater.Application.Repositories
{
    public interface IUserRoleRepository : IAsyncRepository<AppUserRole>
    {
    }
}
