using Theater.Application.Repositories;
using Theater.Domain.Models.Entities;
using Theater.Infrastructure.Concrates;
using Microsoft.EntityFrameworkCore;

namespace Theater.Repository
{
    class CommentRepository : AsyncRepository<Comment>, ICommentRepository
    {
        public CommentRepository(DbContext db) : base(db)
        {
        }
    }
}

