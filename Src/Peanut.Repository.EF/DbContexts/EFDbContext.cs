using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Peanut.Repository.EF
{
    public class EFDbContext : DbContext
    {
        public EFDbContext():
            base("name=xxxx")
        {
            //数据迁移很好的解决了修改表结构，数据丢失的问题
            //自动使用迁移策略
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<EFDbContext, MigrateConfiguration>("xxxxx"));
        }
    }
}
