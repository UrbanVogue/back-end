using Ordering.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Domain.Entities
{
    public class Item : EntityBase
    {
        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public string Color { get; set; }

        public int OrderId { get; set; }

        public Order Order { get; set; }

        public int Quantity { get; set; }

        public double Price { get; set; }

        public string Size { get; set; }
    }
}
