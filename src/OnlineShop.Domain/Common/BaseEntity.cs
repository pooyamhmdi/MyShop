
namespace OnlineShop.Domain.Common;

public abstract class BaseEntity<TKey>
{
    public TKey Id { get; set; } = default!;

    public DateTime CreatedOn { get; set; }

    public DateTime? LastModifiedOn { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? DeletedOn { get; set; }
}

public abstract class BaseEntity : BaseEntity<long>
{
}
