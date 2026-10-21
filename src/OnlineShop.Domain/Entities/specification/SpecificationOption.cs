using OnlineShop.Domain.Common;

namespace OnlineShop.Domain.Entities.specification
{
    public class SpecificationOption : BaseEntity
    {
        public string Value { get; set; } = string.Empty;

        public Specification Specification { get; set; } = null!;
        public long SpecificationId { get; set; }
    }
}
