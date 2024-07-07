
using System.Linq.Expressions;
using Theater.Domain.Models.Entities;
using Theater.Infrastructure.Abstracts;

namespace Theater.Application.Repositories
{
    public interface ISeatRepository : IAsyncRepository<Seat>
    {
        Task HardDeleteAsync(Seat seat, CancellationToken cancellationToken);

    }
}
