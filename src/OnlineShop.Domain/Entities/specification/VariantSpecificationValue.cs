using OnlineShop.Domain.Common;
using OnlineShop.Domain.Entities.product;

namespace OnlineShop.Domain.Entities.specification
{
    public class VariantSpecificationValue : BaseEntity
    {
        public ProductVariant ProductVariant { get; set; } = null!;
        public long ProductVariantId { get; set; }

        public Specification Specification { get; set; } = null!;
        public long SpecificationId { get; set; }

        public SpecificationOption? SpecificationOption { get; set; }
        public long? SpecificationOptionId { get; set; }

        public string? Value { get; set; }
    }
}
