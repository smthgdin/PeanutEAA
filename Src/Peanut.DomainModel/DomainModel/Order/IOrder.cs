using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Peanut.DomainModel.Entity
{
    public interface IOrder
    {
        string OrderCode { get; set; }

        CustomerInfo Customer { get; set; }

        //List<OrderItem>  Items { get; set; }
    }
}
