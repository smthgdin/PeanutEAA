using System;

namespace Peanut.Infrastructure.Mapping
{
    /// <summary>
    /// 
    /// </summary>
    public interface IMapperService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TTo"></typeparam>
        /// <param name="from"></param>
        TTo Map<TTo>(object from);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        object Map(object from, object to);
    }
}