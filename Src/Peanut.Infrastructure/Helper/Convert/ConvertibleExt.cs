/*********************************************************************** 
 * 项目名称 :  Peanut.Helper   
 * 项目描述 :      
 * 类 名 称 :  ConvertibleExt 
 * 说    明 :      
 * 作    者 :  XHT  
 * 创建时间 :  2018/12/01 11:06:18
 * 更新时间 :  2018/12/01 11:06:18 
************************************************************************ 
 * Copyright @   2018. All rights reserved. 
************************************************************************/

using System;

namespace Peanut.Infrastructure.TypeConvert
{
    /// <summary>
    /// 数据转换扩展类类
    /// 
    /// 目的：
    ///     1.支持原生类型和可空类型的转换；
    /// 
    /// 使用规范：
    ///     例如：DateTime? dt = "1981-08-24".ConvertTo&lt;DateTime?&gt;();
    /// </summary>
    public static class ConvertibleExt
    {
        /// <summary>
        /// 数据类型转换扩展方法
        /// </summary>
        /// <typeparam name="T">类型参数</typeparam>
        /// <param name="convertibleValue">实现IConvertible接口的类型</param>
        /// <returns>T指定的类型</returns>
        public static T ConvertTo<T>(this IConvertible convertibleValue)
        {
            if (null == convertibleValue)
                return default(T);

            if (typeof(T).IsGenericType)
            {
                Type genericTypeDefinition = typeof(T).GetGenericTypeDefinition();

                if (genericTypeDefinition == typeof(Nullable<>))
                    return (T)System.Convert.ChangeType(convertibleValue, Nullable.GetUnderlyingType(typeof(T)));
            }
            else
                return (T)System.Convert.ChangeType(convertibleValue, typeof(T));

            throw new InvalidCastException(string.Format("无效转换。不能从类型 \"{0}\" 转成类型 \"{1}\".",
                convertibleValue.GetType().FullName, typeof(T).FullName));
        }

    }
}
