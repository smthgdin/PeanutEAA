using System.Configuration;

namespace Peanut.Infrastructure.Security.Basic
{
    /// <summary>
    /// 自定义Web.Config的节点
    ///
    /// 目的：
    ///     1.外围系统账号管理用
    /// 
    /// 使用规范：
    ///    在配置文件中使用。
    /// </summary>
    public class PeripheralSystemSection : ConfigurationSection
    {
        /// <summary>
        /// 外围系统集合
        /// </summary>
        //设置配置文件中使用的属性名
        [ConfigurationProperty("peripheralSystems", IsDefaultCollection = false)]
        public PeripheralSystemCollection PeripheralSystems
        {
            get
            {
                return this["peripheralSystems"] as PeripheralSystemCollection;
            }
            set
            {
                this["peripheralSystems"] = value;
            }
        }
    }
}