using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.Entity;

namespace Peanut.Repository.EF
{
    /// <summary>
    /// 工作单元
    /// 负责事务管理，实现模型的状态持久化。
    /// 针对单个数据库，或者说单个数据库上下文，非分布式事务管理
    /// 使用场景：EF调用SQL语句、 多个SaveChanges、一次性提交多处修改的情况。EF不会对查询进行事务包装。
    /// </summary>
    public class EFUoW : IUoW, IDisposable
    {
        private static readonly object obj = new object();

        private bool isNeedCommit = true;
        private DbContext db = default(DbContext);
        private DbContextTransaction trans = default(DbContextTransaction);

        public EFUoW(DbContext ctx) 
        {
            db = ctx;
        }

        public void BeginTrans()
        {
            //在EF中，由于每次调用 db.Database.ExecuteSqlCommand的时候,即刻执行了该SQL语句,所以要把他放到一个大的事务中，整体提交、回滚。
            //启动一个事务需要底层数据库连接已打开。因此，如果连接未打开，调用Database.BeginTransaction()会打开连接，在其Dispose时关闭连接。
            trans = db.Database.BeginTransaction();
        }

        public void CommitTrans()
        {
            //如果trans为空，意味着前面没有调用BeginTrans，此时，使用SaveChanges来提交
            if (trans == null)
                db.SaveChanges();
            else
                trans.Commit();

            isNeedCommit = false;
        }

        public void RollbackTrans()
        {
            trans.Rollback();
            isNeedCommit = false;   //回滚了就不需要提交什么了。
        }

        //允许DbContext使用一个EF框架外的事务。
        public void UseTransaction(IDbTransaction currentTransaction)
        {
            //使用外部事务，并且由外部事务来整体提交
            var sqlTrans = currentTransaction as SqlTransaction;
            if (sqlTrans == null)
                throw new InvalidCastException("不能转成SqlTransaction");
            
            db.Database.UseTransaction(sqlTrans);
        }

        public void Dispose()
        {
            //采用using创建uow时，如果不调用CommitTrans方法显式提交，则在跳出代码块时自动提交。
            //非using创建，要显式提交，否则可能存在bug，比如创建uow，调用了begintranscation并且包装了操作，然后不做提交，等gc回收。不确定此时是否在数据库创建事务，还是要等到后面。
            if (isNeedCommit == true)
                CommitTrans();

            if (db != null)
            {
                lock (obj)
                {
                    if (db != null)
                    {
                        trans.Dispose();
                        db.Dispose();
                    }
                }
            }
        }
    }
}
