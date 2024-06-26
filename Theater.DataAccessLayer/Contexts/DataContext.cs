using Microsoft.EntityFrameworkCore;
using Theater.Domain.Models.Entities;
using Theater.Infrastructure.Abstracts;
using Theater.Infrastructure.Concrates;

public class DataContext : DbContext
{
    private readonly IIdentityService identityService;

    public DataContext(DbContextOptions<DataContext> options, IIdentityService identityService)
        : base(options)
    {
        this.identityService = identityService;
    }

    public DbSet<Seat> Seats { get; set; }
    public DbSet<ShowDate> ShowDates { get; set; }
    public DbSet<Poster> Posters { get; set; }
    public DbSet<Hall> Halls { get; set; }
    public DbSet<SeatReservation> SeatReservations { get; set; } // Add this line


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DataContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var changes = this.ChangeTracker.Entries<AuditableEntity>();

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
                        entry.Entity.CreatedAt = DateTime.UtcNow;
                        break;
                    case EntityState.Modified:
                        entry.Property(m => m.CreatedBy).IsModified = false;
                        entry.Property(m => m.CreatedAt).IsModified = false;
                        entry.Entity.LastModifiedBy = identityService.GetPrincipialId();
                        entry.Entity.LastModifiedAt = DateTime.UtcNow;
                        break;
                    case EntityState.Deleted:
                        entry.State = EntityState.Modified;
                        entry.Property(m => m.CreatedBy).IsModified = false;
                        entry.Property(m => m.CreatedAt).IsModified = false;
                        entry.Property(m => m.LastModifiedBy).IsModified = false;
                        entry.Property(m => m.LastModifiedAt).IsModified = false;
                        entry.Entity.DeletedBy = identityService.GetPrincipialId();
                        entry.Entity.DeletedAt = DateTime.UtcNow;
                        break;
                    default:
                        break;
                }
            }
        }
        return base.SaveChangesAsync(cancellationToken);
    }
}
