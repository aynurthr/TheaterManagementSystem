using Microsoft.EntityFrameworkCore;
using Theater.Application.Repositories;
using Theater.Domain.Models.Entities;
using Theater.Infrastructure.Concrates;

namespace Theater.Repository
{


    class TeamMemberRepository : AsyncRepository<TeamMember>, ITeamMemberRepository
    {
        public TeamMemberRepository(DbContext db) : base(db)
        {
        }
    }
}
