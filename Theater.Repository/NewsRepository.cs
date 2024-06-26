using Theater.Application.Repositories;
using Theater.Domain.Models.Entities;
using Theater.Infrastructure.Concrates;
using Microsoft.EntityFrameworkCore;

namespace Theater.Repository
{
    class NewsRepository : AsyncRepository<News>, INewsRepository
    {
        public NewsRepository(DbContext db) : base(db)
        {
        }
    }
}

