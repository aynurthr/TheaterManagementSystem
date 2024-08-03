using Theater.Application.Repositories;
using Theater.Domain.Models.Entities;
using Theater.Infrastructure.Concrates;
using Microsoft.EntityFrameworkCore;

namespace Theater.Repository
{
    class NewsletterRepository : AsyncRepository<Newsletter>, INewsletterRepository
    {
        public NewsletterRepository(DbContext db) : base(db)
        {
        }
    }
}

