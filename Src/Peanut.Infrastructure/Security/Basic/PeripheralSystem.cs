using System.Configuration;

namespace Peanut.Infrastructure.Security.Basic
{
    /// <summary>
    /// 自定义webconfig元素（user、password、sys）
    ///
    /// 目的：
    ///     1.外围系统账号管理用
    /// 
    /// 使用规范：
    ///    在配置文件中使用；用在自定义节点下。
    /// </summary>
    public class PeripheralSystem : ConfigurationElement
    {
        /// <summary>
        /// 用户
        /// </summary>
        //设置配置文件中使用的属性名
        [ConfigurationProperty("user", IsRequired = true)]
        public string User
        {
            get { return (string)this["user"]; }
            set { this["user"] = value; }
        }

        /// <summary>
        /// 密码
        /// </summary>
        //设置配置文件中使用的属性名
        [ConfigurationProperty("password", IsRequired = true)]
        public string Password
        {
            get { return (string)this["password"]; }
            set { this["password"] = value; }
        }

        /// <summary>
        /// 外围系统
        /// </summary>
        //设置配置文件中使用的属性名
        [ConfigurationProperty("sys", IsRequired = true)]
        public string System
        {
            get { return (string)this["sys"]; }
            set { this["sys"] = value; }
        }
    }
}