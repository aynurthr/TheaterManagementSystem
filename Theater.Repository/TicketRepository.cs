using Theater.Application.Repositories;
using Theater.Domain.Models.Entities;
using Theater.Infrastructure.Concrates;
using Microsoft.EntityFrameworkCore;

namespace Theater.Repository
{
    class TicketRepository : AsyncRepository<Ticket>, ITicketRepository
    {
        public TicketRepository(DbContext db) : base(db)
        {
        }
    }
}

