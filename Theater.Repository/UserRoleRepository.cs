using Theater.Application.Repositories;
using Theater.Domain.Models.Entities;
using Theater.Infrastructure.Concrates;
using Microsoft.EntityFrameworkCore;
using Theater.Domain.Models.Entities.Membership;

namespace Theater.Repository
{
    class UserRoleRepository : AsyncRepository<AppUserRole>, IUserRoleRepository
    {
        public UserRoleRepository(DbContext db) : base(db)
        {
        }
    }
}

