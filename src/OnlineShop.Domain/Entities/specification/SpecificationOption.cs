using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.Domain.Entities.specification
{
    public class SpecificationOption
    {
        public long Id { get; set; }
        public string Value { get; set; } = string.Empty;
        public Specification Specification { get; set; } = null;
        public long SpecificationId {get; set;}

    }
}