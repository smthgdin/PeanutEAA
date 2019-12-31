using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Peanut.DomainModel.Entity
{
    public class OrderItem : BaseEntity<int>, IOrderItem
    {
        public string GoodsNode { get; set; }

        public string GoodsType { get; set; }

        public decimal Price { get; set; }

        public int Count { get; set; }

        public decimal Amount { get; set; }

        public int GoodsId { get; set; }
    }
}
