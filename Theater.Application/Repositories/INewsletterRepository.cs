

using Theater.Domain.Models.Entities;
using Theater.Infrastructure.Abstracts;

namespace Theater.Application.Repositories
{
    public interface INewsletterRepository : IAsyncRepository<Newsletter>
    {
    }
}


