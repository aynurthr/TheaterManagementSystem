using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Theater.Application.Modules.PosterModule.Queries.PosterBuyTicketRequestDto;
using Theater.Domain.Models.Entities;
using Theater.Infrastructure.Abstracts;

namespace Theater.Application.Repositories
{
    public interface ISeatRepository : IAsyncRepository<Seat>
    {

        Task<IEnumerable<SeatDto>> GetSeatsByPosterIdAsync(int posterId, CancellationToken cancellationToken);
        Task<IEnumerable<Seat>> GetSeatsByShowDateIdAsync(int showDateId, CancellationToken cancellationToken);
        Task<IEnumerable<int>> GetReservedSeatIdsByShowDateIdAsync(int showDateId, CancellationToken cancellationToken);
    }
}
