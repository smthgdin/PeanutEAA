using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Peanut.DomainModel.Entity
{
    public interface IAddress
    {
        string Country { get; set; }

        string Province { get; set; }

        string City { get; set; }
    }
}
