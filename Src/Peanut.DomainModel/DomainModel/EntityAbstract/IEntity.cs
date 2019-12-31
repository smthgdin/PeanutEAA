using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Peanut.DomainModel
{
    public interface IEntity<K>
    {
        K Id { get; set; }
    }
}
