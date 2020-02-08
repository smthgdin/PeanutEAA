using System;
using System.Net.Http;
using System.Text;

using Newtonsoft.Json;

namespace Peanut.Infrastructure.Extension.JSON
{
    /// <summary>
    /// JSON扩展类
    /// 
    /// 目的：
    ///     1.实现JSON和对象的互转
    /// 
    /// 使用规范：
    ///     
    /// </summary>
    public static class JSONExtension
    {
        /// <summary>
        /// 将Object对象转换成HttpResponseMessage
        /// </summary>
        /// <param name="obj">Object对象</param>
        /// <returns>HttpResponseMessage</returns>
        public static HttpResponseMessage ToJSONResult(this object obj)
        {
            string jsonString;

            if (obj is String || obj is Char)
                jsonString = obj.ToString();
            else
                jsonString = JsonConvert.SerializeObject(obj);

            return 
                new HttpResponseMessage { Content = new StringContent(jsonString, Encoding.GetEncoding("UTF-8"), "application/json") };
        }

        /// <summary>
        /// 将Object对象转换成JSON字符串
        /// </summary>
        /// <param name="obj">Object对象</param>
        /// <returns>JSON字符串</returns>
        public static string ToJSONString(this object obj)
        {
            if (obj is ValueType)
                return string.Empty;

            return JsonConvert.SerializeObject(obj);
        }

        /// <summary>
        /// 将JSON字符串转成类型对象实例
        /// </summary>
        /// <typeparam name="TDestination">类型对象</typeparam>
        /// <param name="jsonString">JSON字符串</param>
        /// <returns>类型对象实例</returns>
        public static TDestination ToObject<TDestination>(this string jsonString) 
            where TDestination : new()
        {
            if (string.IsNullOrWhiteSpace(jsonString))
                return default(TDestination);

            return JsonConvert.DeserializeObject<TDestination>(jsonString);
        }
    }
}
