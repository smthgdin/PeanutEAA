/*********************************************************************** 
 * 项目名称 :  Peanut.Helper 
 * 项目描述 :      
 * 类 名 称 :  GZip 
 * 说    明 :      
 * 作    者 :  XHT 
 * 创建时间 :  2018/11/28 10:15:29 
 * 更新时间 :  2018/11/28 10:15:29  
************************************************************************ 
 * Copyright @   2018. All rights reserved. 
************************************************************************/

using System;
using System.IO;

using ICSharpCode.SharpZipLib.GZip;

namespace Peanut.Helper.Zip
{
    /// <summary>
    /// GZipHelper类
    /// 基于GZip压缩和解压，gzip在压缩文件中的数据时采用的是zlib压缩
    /// 
    /// 目的：
    ///     1.GZip压缩和解压
    ///     2.单个文件压缩
    ///      
    /// 使用规则：
    ///     略
    /// </summary>
    public sealed class GZipHelper
    {
        /// <summary>
        /// 压缩文件
        /// </summary>
        /// <param name="resourceFileName">源文件名（包含路径）</param>
        /// <param name="targetZipName">压缩文件名</param>
        /// <returns></returns>
        public static bool CreateZipFile(string resourceFileName, string targetZipName)
        {
            if (string.IsNullOrWhiteSpace(resourceFileName))
                throw new ArgumentNullException("resourceFileName");

            if (string.IsNullOrWhiteSpace(targetZipName))
                throw new ArgumentNullException("targetZipName");

            if (Path.GetExtension(targetZipName).ToLower() != ".gz")
                throw new ArgumentException("Formate of extension is error");

            FileStream fStream = default(FileStream);
            Stream gzOutputStream = default(Stream);

            try
            {
                fStream = File.OpenRead(resourceFileName);
                byte[] writeData = new byte[fStream.Length];
                fStream.Read(writeData, 0, (int)fStream.Length);

                gzOutputStream = new GZipOutputStream(File.Create(targetZipName));
                gzOutputStream.Write(writeData, 0, writeData.Length);
            }
            finally
            {
                if (fStream != null)
                {
                    fStream.Flush();
                    fStream.Close();
                }

                if (gzOutputStream != null)
                {
                    gzOutputStream.Flush();
                    gzOutputStream.Close();
                }
            }

            return File.Exists(targetZipName);
        }

        /// <summary>
        /// 解压文件
        /// </summary>
        /// <param name="resourceZipName">压缩文件名（包含路径）</param>
        /// <param name="targetFileName">目标文件名</param>
        /// <returns></returns>
        public static bool ExtractZipFile(string resourceZipName, string targetFileName)
        {
            if (string.IsNullOrWhiteSpace(resourceZipName))
                throw new ArgumentNullException("resourceZipName");

            if (string.IsNullOrWhiteSpace(targetFileName))
                throw new ArgumentNullException("targetFileName");

            if (Path.GetExtension(resourceZipName).ToLower() != ".gz")
                throw new ArgumentException("Formate of extension is error");

            Stream gzOutputStream = default(Stream);
            FileStream fStream = default(FileStream);

            try
            {
                gzOutputStream = new GZipInputStream(File.OpenRead(resourceZipName));
               
                //根据目标路径创建或覆盖文件
                fStream = File.Create(targetFileName);

                int size = 2048;                                        //指定压缩块的大小，一般为2048的倍数 
                byte[] writeData = new byte[size];                      //指定缓冲区的大小 

                while (true)
                {
                    size = gzOutputStream.Read(writeData, 0, size);     //读入一个压缩块 

                    if (size > 0)
                    {
                        fStream.Write(writeData, 0, size);              //写入解压文件代表的文件流 
                    }
                    else
                        break;
                }
            }
            finally
            {
                if (fStream != null)
                {
                    fStream.Flush();
                    fStream.Close();
                }

                if (gzOutputStream != null)
                {
                    gzOutputStream.Flush();
                    gzOutputStream.Close();
                }
            }

            return File.Exists(targetFileName);
        }
    }
}
