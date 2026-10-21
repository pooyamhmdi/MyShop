using OnlineShop.Domain.Common;
using OnlineShop.Domain.Entities.category;

namespace OnlineShop.Domain.Entities.specification
{
    public enum SpecificationType
    {
        Text,
        Number,
        Boolean,
        Select
    }

    public class Specification : BaseEntity
    {
        public string Name { get; set; } = string.Empty;

        public SpecificationType Type { get; set; }

        public Category Category { get; set; } = null!;

        public long CategoryId { get; set; }

        public ICollection<SpecificationOption> Options { get; set; }
            = new List<SpecificationOption>();
    }
}
