
using Theater.Application.Repositories;
using Theater.Domain.Models.Entities;
using Theater.Infrastructure.Concrates;
using Microsoft.EntityFrameworkCore;

namespace Theater.Repository
{
    class HallRepository : AsyncRepository<Hall>, IHallRepository
    {
        public HallRepository(DbContext db) : base(db)
        {
        }
    }
}


