using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Peanut.DomainModel
{
    //public interface IModificationProfile<N> where N : struct
    public interface IModificationProfile<N>
    {
        N Modifier { get; set; }   //可控暂时不知道怎么弄。

        DateTime? LastModificationTime { get; set; }    
    }
}
