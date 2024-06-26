using Theater.Domain.Models.Entities;
using Theater.Infrastructure.Abstracts;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Theater.Application.Repositories;
using Theater.Infrastructure.Concrates;

namespace Theater.Repository
{
    public class PosterRepository : AsyncRepository<Poster>, IPosterRepository
    {
        public PosterRepository(DbContext db) : base(db)
        {
        }

        public async Task<Poster> GetByIdAsync(int posterId, CancellationToken cancellationToken) // Implement this method
        {
            return await db.Set<Poster>().FirstOrDefaultAsync(p => p.Id == posterId, cancellationToken);
        }

        public async Task<ShowDate> GetShowDateByIdAsync(int showDateId, CancellationToken cancellationToken)
        {
            return await db.Set<ShowDate>().FirstOrDefaultAsync(sd => sd.Id == showDateId, cancellationToken);
        }

        public async Task<IEnumerable<ShowDate>> GetShowDatesByPosterIdAsync(int posterId, CancellationToken cancellationToken)
        {
            return await db.Set<ShowDate>().Where(sd => sd.PosterId == posterId).ToListAsync(cancellationToken);
        }
    }
}
