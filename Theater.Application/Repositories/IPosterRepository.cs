using Theater.Domain.Models.Entities;
using System.Threading;
using System.Threading.Tasks;
using Theater.Infrastructure.Abstracts;

namespace Theater.Application.Repositories
{
    public interface IPosterRepository : IAsyncRepository<Poster>
    {
        Task<IEnumerable<ShowDate>> GetShowDatesByPosterIdAsync(int posterId, CancellationToken cancellationToken);
        Task<ShowDate> GetShowDateByIdAsync(int showDateId, CancellationToken cancellationToken);
        Task<Poster> GetByIdAsync(int posterId, CancellationToken cancellationToken); // Add this method
    }
}
