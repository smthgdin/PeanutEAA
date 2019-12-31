using System.Data.Entity.Migrations;

namespace Peanut.Repository.EF
{
    public sealed class MigrateConfiguration : DbMigrationsConfiguration<EFDbContext>
    {
        public MigrateConfiguration()
        {
            AutomaticMigrationsEnabled = true;              //启用自动迁移
            AutomaticMigrationDataLossAllowed = true;       //表结构修改、新增、删除表都需要开启允许修改表结构
        }
    }
}
