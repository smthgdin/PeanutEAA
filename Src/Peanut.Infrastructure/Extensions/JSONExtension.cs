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
        public static HttpResponseMessage ToJSON(this object obj)
        {
            string jsonString;

            if (obj is String || obj is Char)
                jsonString = obj.ToString();
            else
                jsonString = JsonConvert.SerializeObject(obj);

            return 
                new HttpResponseMessage { Content = new StringContent(jsonString, Encoding.GetEncoding("UTF-8"), "application/json") };
        }

        public static string ToJSONString(this object obj)
        {
            if (obj is ValueType)
                return string.Empty;

            return JsonConvert.SerializeObject(obj);
        }

        public static T ToObject<T>(this string jsonString) where T : new()
        {
            if (string.IsNullOrWhiteSpace(jsonString))
                return default(T);

            return JsonConvert.DeserializeObject<T>(jsonString);
        }
    }
}
