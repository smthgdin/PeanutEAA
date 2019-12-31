using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Peanut.Repository.EF
{
    public interface IUoW
    {
        void BeginTrans();

        void CommitTrans();

        void RollbackTrans();
    }
}
