using System.Configuration;

namespace Peanut.Infrastructure.Security.Basic
{
    /// <summary>
    /// 自定义元素集合
    ///
    /// 目的：
    ///     1.外围系统账号管理用
    /// 
    /// 使用规范：
    ///    在配置文件中使用；用在自定义节点下。
    /// </summary>
    public class PeripheralSystemCollection : ConfigurationElementCollection
    {
        /// <summary>
        /// 创建新元素
        /// </summary>
        /// <returns></returns>
        protected override ConfigurationElement CreateNewElement()
        {
            return new PeripheralSystem();
        }

        /// <summary>
        /// 元素key
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((PeripheralSystem)element).User;
        }

        /// <summary>
        /// 所索引器
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public PeripheralSystem this[string name]
        {
            get
            {
                return BaseGet(name) as PeripheralSystem;
            }
        }

        /// <summary>
        /// 元素名
        /// </summary>
        protected override string ElementName
        {
            get
            {
                return "peripheralSystem";
            }
        }
    }
}