/*********************************************************************** 
 * 项目名称 :  Peanut.Helper 
 * 项目描述 :      
 * 类 名 称 :  FileHelper 
 * 说    明 :      
 * 作    者 :  XHT 
 * 创建时间 :  2018/12/11 10:15:33 
 * 更新时间 :  2018/12/11 10:15:33
************************************************************************ 
 * Copyright @   2018. All rights reserved. 
************************************************************************/

using System;
using System.Text;
using System.IO;

namespace Peanut.Infrastructure.IO
{
    /// <summary>
    /// 文本文件工具类
    /// 
    /// 目的：
    ///     提供针对文本通用的操作。包括：判断文件是否存在，创建文件，读取文件，删除文件，编辑文件，移动文件和拷贝文件等。
    /// 
    /// 使用规范：
    ///     略
    /// </summary>
    public sealed class FileHelper
    {
        /// <summary>
        /// 判断文件是否存在
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <returns>true : 存在  false：不存在</returns>
        public static bool Exists(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                throw new ArgumentNullException();

            return File.Exists(fileName);
        }

        /// <summary>
        /// 新增文件
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="content">文件内容</param>
        /// <returns></returns>
        public static bool CreateFile(string fileName, string content)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                throw new ArgumentNullException();

            if (File.Exists(fileName))
                throw new Exception("File is exists");

            FileStream fStream = new FileStream(fileName, FileMode.Create);
            StreamWriter sWriter = new StreamWriter(fStream);
            sWriter.Write(content);
            sWriter.Close();
            fStream.Close();

            return File.Exists(fileName);
        }

        /// <summary>
        /// 读取文件内容
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string ReadFile(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                throw new ArgumentNullException();

            //获取操作系统的当前 ANSI 代码页的编码,按照默认编码读取;OpenText是按照utf-8编码读取。
            return File.ReadAllText(fileName, Encoding.Default);
        }

        /// <summary>
        /// 移动文件
        /// </summary>
        /// <param name="sourceFileName">源文件</param>
        /// <param name="destFileName">目标文件</param>
        /// <returns></returns>
        public static bool MoveFile(string sourceFileName, string destFileName)
        {
            if (string.IsNullOrWhiteSpace(sourceFileName) || string.IsNullOrWhiteSpace(destFileName))
                throw new ArgumentNullException();

            File.Move(sourceFileName, destFileName);

            return File.Exists(destFileName);
        }

        /// <summary>
        /// 拷贝文件
        /// </summary>
        /// <param name="sourceFileName">源文件</param>
        /// <param name="destFileName">目标文件</param>
        /// <returns></returns>
        public static bool CopyFile(string sourceFileName, string destFileName)
        {
            if (string.IsNullOrWhiteSpace(sourceFileName) || string.IsNullOrWhiteSpace(destFileName))
                throw new ArgumentNullException();

            File.Copy(sourceFileName, destFileName);

            return File.Exists(destFileName);
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <returns></returns>
        public static bool DeleteFile(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                throw new ArgumentNullException();

            File.Delete(fileName);

            return !File.Exists(fileName);
        }
    }
}
