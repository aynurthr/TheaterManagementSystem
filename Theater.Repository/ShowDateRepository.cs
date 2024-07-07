using Theater.Application.Repositories;
using Theater.Domain.Models.Entities;
using Theater.Infrastructure.Concrates;
using Microsoft.EntityFrameworkCore;

namespace Theater.Repository
{
    class ShowDateRepository : AsyncRepository<ShowDate>, IShowDateRepository
    {
        public ShowDateRepository(DbContext db) : base(db)
        {
        }
    }
}

