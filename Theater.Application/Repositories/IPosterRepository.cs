using Theater.Domain.Models.Entities;
using System.Threading;
using System.Threading.Tasks;
using Theater.Infrastructure.Abstracts;
using Microsoft.EntityFrameworkCore;

namespace Theater.Application.Repositories
{
    public interface IPosterRepository : IAsyncRepository<Poster>
    {
       
    }
}
