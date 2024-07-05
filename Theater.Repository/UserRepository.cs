using Theater.Application.Repositories;
using Theater.Domain.Models.Entities;
using Theater.Infrastructure.Concrates;
using Microsoft.EntityFrameworkCore;
using Theater.Domain.Models.Entities.Membership;

namespace Theater.Repository
{
    class UserRepository : AsyncRepository<AppUser>, IUserRepository
    {
        public UserRepository(DbContext db) : base(db)
        {
        }
    }
}

