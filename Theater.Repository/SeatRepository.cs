using Theater.Application.Repositories;
using Theater.Domain.Models.Entities;
using Theater.Infrastructure.Concrates;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Theater.Repository
{
    class SeatRepository : AsyncRepository<Seat>, ISeatRepository
    {
        public SeatRepository(DbContext db) : base(db)
        {
        }

        public async Task HardDeleteAsync(Seat seat, CancellationToken cancellationToken)
        {
            db.Entry(seat).State = EntityState.Deleted;
            await db.SaveChangesAsync(cancellationToken);
        }
    }
}


