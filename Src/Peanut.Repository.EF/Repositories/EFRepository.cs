using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Data.SqlClient;
using Peanut.DomainModel;


namespace Peanut.Repository.EF
{
    /// <summary>
    /// IRepository<typeparamref name="T"/>基于EF技术框架的实现
    /// </summary>
    /// <typeparam name="V">抽象模型实现类，为了简化设计实现类等于数据库实体（即数据表模型）</typeparam>
    /// <typeparam name="T">抽象模型（模型接口）</typeparam>
    public class EFRepository<V, T> : IRepository<T>
        where V : class, T, new()
    {
        private DbContext db;

        //使用构造函数依赖注入
        public EFRepository(DbContext ctx)
        {
            db = ctx;
        }

        public virtual void Add(T model)
        {
            db.Set<V>().Add((V)model);
        }

        public virtual T Create()  //需要附加的update
        {
            return new V();
        }

        public virtual void Delete(object id)
        {
            var entity = db.Set<V>().Find(id);
            db.Set<V>().Remove(entity);
        }

        public virtual void Delete(T model)
        {
            db.Set<V>().Remove((V)model);
        }

        public virtual void Delete(List<T> range)
        {
            range.ForEach(x => db.Set<V>().Remove((V)x));
        }

        public virtual T Get(object id)
        {
            return db.Set<V>().Find(id);
        }

        public virtual ICollection<T> GetCollection()
        {
            Collection<T> list= new Collection<T>();
            var entities =  db.Set<V>();
            entities.ForEachAsync(x => list.Add((T)x));
            return list;
        }

        public virtual DbSet<V> Entitys
        {
            get
            {
                return db.Set<V>();
            }
        }

        public virtual T Get(params object[] keyValues)
        {
            return db.Set<V>().Find(keyValues);
        }

        public virtual void Update(T model, bool isReadFirst = true)
        {
            //判断先读取然后修改，还是直接创建对象保存
            if(isReadFirst)
            {
                //如果使用先查询后更新则因为EF的跟踪机制在，所以可以只更新个别属性，然后直接保存。此方法会将所有属性都更新。
                db.Entry<V>((V)model).State = EntityState.Modified;
                return;
            }

            //附加之后，实体的状态为 EntityState.Unchanged，如果不改变状态，在SaveChanged的时候将什么也不做。
            var attachModel = db.Set<V>().Attach((V)model);
            db.Entry<V>(attachModel).State = EntityState.Modified;
        }

        //public static void Update<TEntity>(this DbContext dbContext, params TEntity[] entities) where TEntity : EntityBase
        //{
        //    if (dbContext == null) throw new ArgumentNullException("dbContext");
        //    if (entities == null) throw new ArgumentNullException("entities");

        //foreach (TEntity entity in entities)
        //{
        //    DbSet<TEntity> dbSet = dbContext.Set<TEntity>();
        //    try
        //    {
        //        DbEntityEntry<TEntity> entry = dbContext.Entry(entity);
        //        if (entry.State == EntityState.Detached)
        //        {
        //            dbSet.Attach(entity);
        //            entry.State = EntityState.Modified;
        //        }
        //    }
        //    catch (InvalidOperationException)
        //    {
        //        TEntity oldEntity = dbSet.Find(entity.Id);
        //        //setValues方法就是把新值设置到旧实体上（这一条很强大,支持任何类型，比如ViewObject,DTO与POCO可以直接映射传值）。
        //        //由于值的更新是直接在上下文中的现有实体上进行的，EF会自己跟踪值的变化，因此这里并不需要我们来强制设置状态为Modified，执行的sql语句也足够简单
        //        dbContext.Entry(oldEntity).CurrentValues.SetValues(entity);
        //    }
        //}
        //}

        public void BulkInsert(List<V> entities)
        {
            //db.Database.Connection
            //using (SqlConnection conn = new SqlConnection())
            //{
            //    conn.ConnectionString = ConnectionString;
            //    if (conn.State != ConnectionState.Open)
            //    {
            //        conn.Open();
            //    }

            //    string tableName = string.Empty;
            //    var tableAttribute = typeof(V).GetCustomAttributes(typeof(TableAttribute), true).FirstOrDefault();
            //    if (tableAttribute != null)
            //        tableName = ((TableAttribute)tableAttribute).Name;
            //    else
            //        tableName = typeof(V).Name;

            //    SqlBulkCopy sqlBC = new SqlBulkCopy(conn)
            //    {
            //        BatchSize = 100000,
            //        BulkCopyTimeout = 0,
            //        DestinationTableName = tableName
            //    };
            //    using (sqlBC)
            //    {
            //        sqlBC.WriteToServer(entities.ToDataTable());
            //    }
            //}
        }
    }
}
