
using OnlineShop.Domain.Entities.product;

namespace OnlineShop.Domain.Entities.specification
{
    public class VariantSpecificationValue
    {
        public long Id { get; set; }

        public long ProductVariantId { get; set; }

        public ProductVariant ProductVariant { get; set; } = null!;

        public long SpecificationId { get; set; }

        public Specification Specification { get; set; } = null!;

        public long? SpecificationOptionId { get; set; }

        public SpecificationOption? SpecificationOption { get; set; }

        public string? Value { get; set; }
    }
}