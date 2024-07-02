

using Theater.Application.Repositories;
using Theater.Domain.Models.Entities;
using Theater.Infrastructure.Concrates;
using Microsoft.EntityFrameworkCore;

namespace Theater.Repository
{
    class ActorRepository : AsyncRepository<Actor>, IActorRepository
    {
        public ActorRepository(DbContext db) : base(db)
        {
        }
    }
}

