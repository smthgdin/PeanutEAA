using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Peanut.DomainModel.Entity
{
    public interface IOrderItem
    {
        string GoodsNode { get; set; }

        string GoodsType { get; set; }

        decimal Price { get; set; }

        int Count { get; set; }

        decimal Amount { get; set; }
    }
}
