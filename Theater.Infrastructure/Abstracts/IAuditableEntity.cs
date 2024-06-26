
namespace Theater.Infrastructure.Abstracts
{
    public interface IAuditableEntity
    {
        int? CreatedBy { get; set; }
        DateTime CreatedAt { get; set; }
        int? LastModifiedBy { get; set; }
        DateTime? LastModifiedAt { get; set; }
        int? DeletedBy { get; set; }
        DateTime? DeletedAt { get; set; }
    }
}
