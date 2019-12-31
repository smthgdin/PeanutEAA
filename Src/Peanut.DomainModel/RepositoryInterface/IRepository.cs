using System.Collections.Generic;

namespace Peanut.DomainModel
{
    /// <summary>
    /// 仓储接口
    /// 
    /// 仓存被设计来管理领域模型、值对象。
    /// 
    /// 暂时不考虑CQS，因为觉得越分越复杂，等以后有必要再弄。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IRepository<T>
    {

        T Create();

        void Add(T model);

        T Get(object id);

        ICollection<T> GetCollection();

        T Get(params object[] keyValues);

        void Update(T model, bool isReadFirst = true);

        void Delete(object id);

        void Delete(T model);

        void Delete(List<T> range);
    }
}
