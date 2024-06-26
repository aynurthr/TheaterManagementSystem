using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Theater.Application.Repositories;
using Theater.Domain.Models.Entities;
using Theater.Infrastructure.Concrates;

namespace Theater.Repository
{
    class ContactPostRepository :  AsyncRepository<ContactPost>, IContactPostRepository
    {
        public ContactPostRepository(DbContext db) : base(db)
        {
        }
    }
}
