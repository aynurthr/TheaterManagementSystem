using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Theater.Application.Modules.PosterModule.Queries.PosterBuyTicketRequestDto;
using Theater.Application.Repositories;
using Theater.Domain.Models.Entities;
using Theater.Infrastructure.Abstracts;
using Theater.Infrastructure.Concrates;

namespace Theater.Repository
{
    public class SeatRepository : AsyncRepository<Seat>, ISeatRepository
    {
        public SeatRepository(DbContext db) : base(db)
        {
        }

        public async Task<IEnumerable<Seat>> GetSeatsByShowDateIdAsync(int showDateId, CancellationToken cancellationToken)
        {
            return await db.Set<Seat>()
                           .Where(s => db.Set<SeatReservation>()
                                         .Where(sr => sr.ShowDateId == showDateId)
                                         .Select(sr => sr.SeatId)
                                         .Contains(s.Id))
                           .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<int>> GetReservedSeatIdsByShowDateIdAsync(int showDateId, CancellationToken cancellationToken)
        {
            return await db.Set<SeatReservation>()
                           .Where(sr => sr.ShowDateId == showDateId)
                           .Select(sr => sr.SeatId)
                           .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<SeatDto>> GetSeatsByPosterIdAsync(int posterId, CancellationToken cancellationToken)
        {
            var poster = await db.Set<Poster>()
                                 .Include(p => p.Hall)
                                 .FirstOrDefaultAsync(p => p.Id == posterId, cancellationToken);

            if (poster == null)
                return Enumerable.Empty<SeatDto>();

            var hall = poster.Hall;
            var seatDtos = new List<SeatDto>();

            var rows = hall.SeatsPerRowJson.Split(',').Select(int.Parse).ToArray();

            for (int row = 1; row <= rows.Length; row++)
            {
                for (int seatNumber = 1; seatNumber <= rows[row - 1]; seatNumber++)
                {
                    seatDtos.Add(new SeatDto
                    {
                        Row = row,
                        Number = seatNumber,
                        Price = poster.Price,
                        IsReserved = false
                    });
                }
            }

            return seatDtos;
        }

       
    }
}
