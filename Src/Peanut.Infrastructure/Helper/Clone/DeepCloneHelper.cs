/*********************************************************************** 
 * 项目名称 :  Peanut.Helper   
 * 项目描述 :      
 * 类 名 称 :  DeepCloneHelper 
 * 说    明 :      
 * 作    者 :  XHT  
 * 创建时间 :  2018/12/01 09:55:13
 * 更新时间 :  2018/12/01 09:55:13  
************************************************************************ 
 * Copyright @   2018. All rights reserved. 
************************************************************************/

using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

namespace Peanut.Infrastructure.Clone
{
    /// <summary>
    /// 深拷贝帮助类（创建了一个原对象的深表副本）
    /// 
    /// 目的：
    ///     1.提供深拷贝功能，方便一些对象的复制，比如列表，字典这类。
    ///     2.提供二进制流的序列化和反序列化功能
    /// 
    /// 使用规范：
    ///     略
    /// </summary>
    public sealed class DeepCloneHelper
    {
        #region 序列化/反序列化 (针对二进制流)

        /// <summary>
        /// 序列化对象成流数据
        /// </summary>
        /// <param name="obj">需要序列化的对象</param>
        /// <returns>二进制流（外部使用完需要关闭流）</returns>
        public static Stream SerializeToBinaryStream(object obj)
        {
            if (obj == null)
                throw new ArgumentNullException("obj");

            IFormatter bf = new BinaryFormatter(null, new StreamingContext(StreamingContextStates.Clone));
            return SerializeToBinaryStream(obj, bf);
        }

        /// <summary>
        /// 将流数据还原成对象
        /// </summary>
        /// <param name="objStream">需要反序列化的对象流</param>
        /// <returns>反序列化后的对象（外部使用完需要关闭流）</returns>
        public static object DeserializeFromBinaryStream(Stream objStream)
        {
            if (objStream == null || objStream.Length == 0)
                throw new ArgumentNullException("objStream");

            IFormatter bf = new BinaryFormatter(null, new StreamingContext(StreamingContextStates.Clone));
            return DeserializeToObject(objStream, bf);
        }

        /// <summary>
        /// 将流数据还原成对象
        /// </summary>
        /// <typeparam name="T">类型参数，指定了反序列成什么类型对象</typeparam>
        /// <param name="objStream">需要反序列化的对象流</param>
        /// <returns>T指定的对象（外部使用完需要关闭流）</returns>
        public static T DeserializeFromBinaryStream<T>(Stream objStream)
        {
            if (objStream == null || objStream.Length == 0)
                throw new ArgumentNullException("objStream");

            IFormatter bf = new BinaryFormatter(null, new StreamingContext(StreamingContextStates.Clone));
            return (T)DeserializeToObject(objStream, bf);
        }

        #endregion

        #region 深拷贝

        /// <summary>
        /// 深拷贝
        /// </summary>
        /// <param name="obj">要克隆的对象</param>
        /// <returns></returns>
        public static object Clone(object obj)
        {
            if (obj == null)
                throw new ArgumentNullException("obj");

            return CloneObj(obj);
        }

        /// <summary>
        /// 深拷贝
        /// </summary>
        /// <typeparam name="T">类型参数，指定克隆后返回什么类型对象</typeparam>
        /// <param name="obj">要克隆的对象</param>
        /// <returns></returns>
        public static T Clone<T>(T obj)
        {
            return (T)CloneObj(obj);
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 深拷贝，二进制序列化
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private static object CloneObj(object obj)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                //定义二进制序列化器用于序列化
                IFormatter bf = new BinaryFormatter(null, new StreamingContext(StreamingContextStates.Clone));
                bf.Serialize(ms, obj);
                ms.Seek(0, SeekOrigin.Begin);

                //反序列化至另一个对象(即创建了一个原对象的深表副本) 
                object CloneObject = bf.Deserialize(ms);

                // 关闭流 
                ms.Close();

                return CloneObject;
            }
        }

        /// <summary>
        /// 获取对象的二进制流数据
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="bf">格式化器</param>
        /// <returns>对象的二进制流(外部用完需要关闭流)</returns>
        private static Stream SerializeToBinaryStream(object obj, IFormatter bf)
        {
            MemoryStream ms = new MemoryStream();
            bf.Serialize(ms, obj);
            ms.Seek(0, SeekOrigin.Begin);

            return ms;
        }

        /// <summary>
        /// 将流数据反序列化成对象
        /// </summary>
        /// <param name="st">对象二进制流</param>
        /// <param name="bf">格式化器</param>
        /// <returns>反序列化后的对象</returns>
        private static object DeserializeToObject(Stream st, IFormatter bf)
        {
            return bf.Deserialize(st);
        }

        #endregion
    }
}
