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

    }
}
