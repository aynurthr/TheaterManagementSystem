using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Theater.Domain.Models.Entities.Membership;
using Theater.Infrastructure.Abstracts;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System;

namespace Theater.DataAccessLayer.Contexts
{
    public class DataContext : IdentityDbContext<AppUser, AppRole, int, AppUserClaim, AppUserRole, AppUserLogin, AppRoleClaim, AppUserToken>
    {
        private readonly IIdentityService identityService;

        public DataContext(DbContextOptions options, IIdentityService identityService)
            : base(options)
        {
            this.identityService = identityService;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DataContext).Assembly);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var changes = this.ChangeTracker.Entries<IAuditableEntity>();

            if (changes != null)
            {
                foreach (var entry in changes.Where(m => m.State == EntityState.Added
                || m.State == EntityState.Modified
                || m.State == EntityState.Deleted))
                {
                    switch (entry.State)
                    {
                        case EntityState.Added:
                            entry.Entity.CreatedBy = identityService.GetPrincipialId();
                            entry.Entity.CreatedAt = DateTime.Now; //UtcNow for global sites
                            break;
                        case EntityState.Modified:
                            entry.Property(m => m.CreatedBy).IsModified = false;
                            entry.Property(m => m.CreatedAt).IsModified = false;
                            entry.Entity.LastModifiedBy = identityService.GetPrincipialId();
                            entry.Entity.LastModifiedAt = DateTime.Now;
                            break;
                        case EntityState.Deleted:
                            entry.State = EntityState.Modified;
                            entry.Property(m => m.CreatedBy).IsModified = false;
                            entry.Property(m => m.CreatedAt).IsModified = false;
                            entry.Property(m => m.LastModifiedBy).IsModified = false;
                            entry.Property(m => m.LastModifiedAt).IsModified = false;
                            entry.Entity.DeletedBy = identityService.GetPrincipialId();
                            entry.Entity.DeletedAt = DateTime.Now;
                            break;
                        default:
                            break;
                    }
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
