

using Theater.Application.Repositories;
using Theater.Domain.Models.Entities;
using Theater.Infrastructure.Concrates;
using Microsoft.EntityFrameworkCore;

namespace Theater.Repository
{
    class GenreRepository : AsyncRepository<Genre>, IGenreRepository
    {
        public GenreRepository(DbContext db) : base(db)
        {
        }
    }
}


