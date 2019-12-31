using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Peanut.DomainModel
{
    public interface ICreationProfile<M>
    {
        M Creator { get; set; }

        DateTime CreationTime { get; set; }
    }
}
