using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using Peanut.DomainModel;

using SqlSugar;

namespace Peanut.Repository.SQLSugar
{
    public class SQLSugarRepository<V, T> : IRepository<T>
        where V : class, T, new()
    {
        protected SimpleClient client;
        protected ConnectionConfig config;


        #region C

        public virtual T Create()
        {
            return new V();
        }

        public void Add(T model)
        {
            client.Insert((V)model);
        }

        #endregion

        #region R

        public virtual T Get(object id)
        {
            return client.GetById<V>(id);
        }

        public ICollection<T> GetCollection()
        {
            return (ICollection<T>)client.GetList<V>();
        }

        public virtual IList<T> GetList(Expression<Func<V, bool>> whereExpression)
        {
            return client.GetList(whereExpression).ToList<T>();
        }

        #endregion

        #region U

        public void Update(T model, bool isReadFirst = true)
        {
            client.Update((model as V));
        }

        public virtual bool Update(Expression<Func<V, V>> columns, Expression<Func<V, bool>> whereExpression)
        {
            return client.Update<V>(columns, whereExpression);
        }

        #endregion

        #region D

        public virtual void Delete(object Id)
        {
            dynamic tmp = Id;
            client.DeleteById(tmp);
        }

        public virtual void Delete(T model)
        {
            client.Delete((model as V));
        }

        public T Get(params object[] keyValues)
        {
            throw new NotImplementedException();
        }

        public void Delete(List<T> range)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
