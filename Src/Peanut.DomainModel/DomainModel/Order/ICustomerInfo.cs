using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Peanut.DomainModel.Entity
{
    public interface ICustomerInfo
    {
        string Name { get; set; }

        string Moile { get; set; }

        string Address { get; set; }
    }
}
