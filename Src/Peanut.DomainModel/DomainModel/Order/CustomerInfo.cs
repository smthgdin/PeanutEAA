using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Peanut.DomainModel.Entity
{
    public class CustomerInfo : BaseEntity<int>, ICustomerInfo
    {
        public string Name { get; set; }

        public string Moile { get; set; }

        public string Address { get; set; }

        public int CustomerId { get; set; }
    }
}
