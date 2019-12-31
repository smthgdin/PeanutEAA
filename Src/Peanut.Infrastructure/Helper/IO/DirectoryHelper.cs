/*********************************************************************** 
 * 项目名称 :  Peanut.Helper 
 * 项目描述 :      
 * 类 名 称 :  DirectoryHelper 
 * 说    明 :      
 * 作    者 :  XHT 
 * 创建时间 :  2018/12/11 14:09:53
 * 更新时间 :  2018/12/11 14:09:53 
************************************************************************ 
 * Copyright @   2018. All rights reserved. 
************************************************************************/

using System;
using System.IO;


namespace Peanut.Infrastructure.IO
{
    /// <summary>
    /// 文件夹工具类
    /// 
    /// 目的：
    ///     提供针对文件夹通用的操作。包括：判断文件夹是否存在，创建文件夹，删除文件夹等。
    /// 
    /// 使用规范：
    ///     略
    /// </summary>
    public sealed class DirectoryHelper
    {
        /// <summary>
        /// 判断文件夹是否存在
        /// </summary>
        /// <param name="directoryPath">文件夹路径</param>
        /// <returns>true ：存在  false ：不存在</returns>
        public static bool Exists(string directoryPath)
        {
            if (string.IsNullOrWhiteSpace(directoryPath))
                throw new ArgumentNullException();

            return Directory.Exists(directoryPath);
        }

        /// <summary>
        /// 新增文件夹
        /// </summary>
        /// <param name="directoryPath">文件夹路径</param>
        /// <returns></returns>
        public static bool CreateDirectory(string directoryPath)
        {
            if (string.IsNullOrWhiteSpace(directoryPath))
                throw new ArgumentNullException();

            Directory.CreateDirectory(directoryPath);

            return Directory.Exists(directoryPath);
        }

        /// <summary>
        /// 删除文件夹
        /// </summary>
        /// <param name="directoryPath">文件夹路径</param>
        public static void DeleteDirectory(string directoryPath)
        {
            if (string.IsNullOrWhiteSpace(directoryPath))
                throw new ArgumentNullException();

            Directory.Delete(directoryPath);
        }

        /// <summary>
        /// 获取文件夹里的文件
        /// </summary>
        /// <param name="directoryPath">文件夹路径</param>
        /// <returns>文件名数组</returns>
        public static string[] GetFiles(string directoryPath)
        {
            if (string.IsNullOrWhiteSpace(directoryPath))
                throw new ArgumentNullException();

            return Directory.GetFiles(directoryPath);
        }

        /// <summary>
        /// 获取文件夹里的子文件夹
        /// </summary>
        /// <param name="directoryPath">文件夹路径</param>
        /// <returns>子文件夹名称数组</returns>
        public static string[] GetDirectories(string directoryPath)
        {
            if (string.IsNullOrWhiteSpace(directoryPath))
                throw new ArgumentNullException();

            return Directory.GetDirectories(directoryPath);
        }
    }
}
