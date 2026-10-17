using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OnlineShop.Domain.Entities.category;

namespace OnlineShop.Domain.Entities.specification
{
    public enum SpicificationType
    {
        Text,
        Number,
        Boolean,
        Select
    }
    public class Specification
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public SpicificationType Type { get; set; }
        public Category Category { get; set; }
        public long CategoryId { get; set; }
        public ICollection<SpecificationOption> Options {get;set;}
        = new List<SpecificationOption>();
    }
}