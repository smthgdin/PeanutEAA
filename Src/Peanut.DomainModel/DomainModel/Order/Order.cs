using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Peanut.DomainModel.Entity
{
    public class Order : BaseEntity<int>, IOrder
    {
        public string OrderCode { get; set; }

        public CustomerInfo Customer { get; set; }
    }
}
