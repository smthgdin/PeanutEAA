using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Peanut.DomainModel.Entity
{
    /// <summary>
    /// 实体基类
    /// </summary>
    /// <typeparam name="K">主键的数据类型</typeparam>
    public class BaseEntity<K>
    {
        /// <summary>
        /// 所有实体都有Id属性，作为实体的主键
        /// </summary>
        public K Id { get; set; }
    }
}
