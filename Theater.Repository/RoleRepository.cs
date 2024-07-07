
using Theater.Application.Repositories;
using Theater.Domain.Models.Entities;
using Theater.Infrastructure.Concrates;
using Microsoft.EntityFrameworkCore;

namespace Theater.Repository
{
    class RoleRepository : AsyncRepository<Role>, IRoleRepository
    {
        public RoleRepository(DbContext db) : base(db)
        {
        }
    }
}

