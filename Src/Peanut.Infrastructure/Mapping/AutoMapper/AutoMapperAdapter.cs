using System;
using System.Linq;
using System.Reflection;

using AutoMapper;

namespace Peanut.Infrastructure.Mapping.AutoMapper
{
    /// <summary>
    /// 自动映射类
    /// 
    /// 目的：
    ///     1.将DTO数据转成CRM实体
    ///     2.将CRM实体转成DTO数据
    /// 
    /// 使用规范：
    ///     
    /// </summary>
    public class AutoMapperAdapter : IMapperService
    {
        public void Init(Type type)
        {
            var profiles = type.Assembly.GetTypes()
                .Where(s => s.Name.Contains("Profile") && s.BaseType.Name.Equals("Profile")).ToList();

            foreach(var profile in profiles)
            {
                var mapProfile = Activator.CreateInstance(profile);
                Mapper.Initialize(x => x.AddProfile((Profile)mapProfile));
            }
        }

        public TTo Map<TTo>(object from)
        {
            return Mapper.Map<TTo>(from);
        }

        public object Map(object from, object to)
        {
            return Mapper.Map(from, to);
        }
    }
}