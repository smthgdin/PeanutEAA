/*********************************************************************** 
 * 项目名称 :  Peanut.Helper  
 * 项目描述 :      
 * 类 名 称 :  Zip 
 * 说    明 :      
 * 作    者 :  XHT 
 * 创建时间 :  2018/11/28 9:29:02 
 * 更新时间 :  2019/01/07 17:29:45  
************************************************************************ 
 * Copyright @   2018. All rights reserved. 
************************************************************************/

using System;
using System.Text;
using System.Collections.Generic;
using System.IO;

using ICSharpCode.SharpZipLib.Zip;

namespace Peanut.Helper.Zip
{
    /// <summary>
    /// ZipHelper类
    /// 基于Zip压缩和解压
    /// 
    /// 
    /// 目的：
    ///     1.单个或者多个文件压缩和解压
    ///     2.对压缩文件进行修改操作
    ///     3.设置压缩密码
    /// 
    /// 使用规则：
    ///     略
    /// </summary>
    public sealed class ZipHelper
    {
        #region  Zip And UnZip

        /// <summary>
        /// 压缩指定目录下的文件
        /// </summary>  
        /// <param name="resourceDirectory">需要压缩的文件夹</param>
        /// <param name="targetZipName">压缩文件名（包括路径）</param>
        /// <param name="password">压缩文件密码</param>
        /// <param name="isRecurse">是否需要压缩路径包含的文件夹</param>
        /// <param name="fileFilter">需要压缩的文件类型（格式：.txt 或 .jpg 等）</param>
        /// <returns></returns>
        public static bool CreateZipFile(string resourceDirectory, string targetZipName, string password = "",
            bool isRecurse = true, string fileFilter = "")
        {
            if (string.IsNullOrWhiteSpace(resourceDirectory))
                throw new ArgumentNullException("resourceDirectory");

            if (string.IsNullOrWhiteSpace(targetZipName))
                throw new ArgumentNullException("targetZipName");
            if (Path.GetExtension(targetZipName).ToLower() != ".zip")
                throw new ArgumentException("Formate of extension is error");

            FastZip fZip = new FastZip();

            if (password.Length > 0) 
                fZip.Password = password;

            fZip.CreateZip(targetZipName, resourceDirectory, isRecurse, fileFilter);

            return File.Exists(targetZipName);
        }

        /// <summary>
        /// 解压文件
        /// </summary>
        /// <param name="resourceZipName">压缩文件</param>
        /// <param name="targetDirectory">放置解压文件的文件夹</param>
        /// <param name="password">压缩文件密码</param>
        /// <param name="fileFilter">文件过滤器，输入只解压的类型</param>
        /// <returns></returns>
        public static bool UnZipFile(string resourceZipName, string targetDirectory, 
            string password = "", string fileFilter = "")
        {
            if (string.IsNullOrWhiteSpace(resourceZipName))
                throw new ArgumentNullException("resourceZipName");
            if (Path.GetExtension(resourceZipName).ToLower() != ".zip")
                throw new ArgumentException("Formate of extension is error");

            if (string.IsNullOrWhiteSpace(targetDirectory))
                throw new ArgumentNullException("targetDirectory");
            if (!Directory.Exists(targetDirectory))
                throw new DirectoryNotFoundException("TargetDirectory is Not Find");

            int beforeObjects = 
                Directory.GetFiles(targetDirectory).Length + Directory.GetDirectories(targetDirectory).Length;

            FastZip fZip = new FastZip();

            if (password.Length > 0)
                fZip.Password = password;

            fZip.ExtractZip(resourceZipName, targetDirectory, fileFilter);

            int afterObjects = 
                Directory.GetFiles(targetDirectory).Length + Directory.GetDirectories(targetDirectory).Length;

            return afterObjects > beforeObjects;
        }

        #endregion

        #region Add Or Delete Files In ZipFile

        /// <summary>
        /// 往存在的压缩文件里添加文件
        /// 单独或者成批添加
        /// </summary>
        /// <param name="zipName">压缩文件名（包含路径）</param>
        /// <param name="fileNameList">添加文件列表（包含路径）</param>
        /// <returns></returns>
        public static bool AddFilesToZip(string zipName, List<string> fileNameList)
        {
            if (string.IsNullOrWhiteSpace(zipName))
                throw new ArgumentNullException("zipName");

            if (fileNameList == null || fileNameList.Count == 0)
                throw new ArgumentException("fileNameList is null or count of item is zero");

            if (Path.GetExtension(zipName).ToLower() != ".zip")
                throw new ArgumentException("Formate of extension is error");

            long beforeFiles;
            long afterFiles;

            using (ZipFile zFile = new ZipFile(zipName))
            {
                beforeFiles = zFile.Count;

                zFile.BeginUpdate();

                foreach (string fileName in fileNameList)
                {
                    zFile.Add(fileName);
                }

                zFile.CommitUpdate();

                afterFiles = zFile.Count;
            }

            return afterFiles > beforeFiles;
        }

        /// <summary>
        /// 删除压缩文件里的文件
        /// </summary>
        /// <param name="zipName">压缩文件名（包含路径）</param>
        /// <param name="fileNameList">文件名列表</param>
        /// <returns></returns>
        public static bool DeleteFilesFromZip(string zipName, List<string> fileNameList)
        {
            if (string.IsNullOrWhiteSpace(zipName))
                throw new ArgumentNullException("zipName");

            if (fileNameList == null || fileNameList.Count == 0)
                throw new ArgumentException("fileNameList is null or count of item is zero");

            if (Path.GetExtension(zipName).ToLower() != ".zip")
                throw new ArgumentException("Formate of extension is error");

            long beforeFiles;
            long afterFiles;

            using (ZipFile zFile = new ZipFile(zipName))
            {
                beforeFiles = zFile.Count;

                zFile.BeginUpdate();

                foreach (string fileName in fileNameList)
                {
                    zFile.Delete(fileName);
                }

                zFile.CommitUpdate();

                afterFiles = zFile.Count;
            }

            return afterFiles < beforeFiles;
        }

        #endregion
    }

    /// <summary>
    /// ZipHelper的扩展类
    /// 
    /// 
    /// 目的：
    ///     实现文件夹压缩和设置压缩密码的链式操作。（如：new DirectoryInfo("...").ToZip().SetPassword("fits")）
    /// 
    /// 使用规则：
    ///     略
    /// </summary>
    public static class ZipHelperExt
    {
        /// <summary>
        /// 将文件夹压缩成Zip文件
        /// 压缩文件和被压缩文件在同一目录
        /// </summary>
        /// <param name="directory"></param>
        /// <param name="password">压缩文件密码</param>
        /// <param name="isRecurse">是否需要压缩路径包含的文件夹</param>
        /// <param name="fileFilter">需要压缩的文件类型（格式：.txt 或 .jpg 等）</param>
        /// <returns></returns>
        [Obsolete("该方法已过时")]
        public static void ToZip(this DirectoryInfo directory, string password = "", 
            bool isRecurse = true, string fileFilter = "")
        {
            if (directory.Parent == null)
                throw new NullReferenceException("Parent Directory is null");

            StringBuilder zipNameBuilder = new StringBuilder();
            zipNameBuilder.Append(directory.Parent.FullName);
            zipNameBuilder.Append(directory.Name);
            zipNameBuilder.Append(".zip");
            string zipName = zipNameBuilder.ToString();

            FastZip fZip = new FastZip();

            if (password.Length > 0)
                fZip.Password = password;

            fZip.CreateZip(zipName, directory.FullName, isRecurse, fileFilter);
        }

        /// <summary>
        /// 将文件夹压缩成Zip文件
        /// </summary>
        /// <param name="directory">针对DirectoryInfo的扩展</param>
        /// <param name="zipFileFullName">压缩文件全名</param>
        /// <param name="password">压缩文件密码</param>
        /// <param name="isRecurse">是否需要压缩路径包含的文件夹</param>
        /// <param name="fileFilter">需要压缩的文件类型（格式：.txt 或 .jpg 等）</param>
        public static void Zip(this DirectoryInfo directory, string zipFileFullName = "", string password = "",
           bool isRecurse = true, string fileFilter = "")
        {
            //如果没有指定目标文件全名或者只有盘符信息，则生成到和压缩文件同一目录下
            if (zipFileFullName.Length == 0 || zipFileFullName.EndsWith(":\\"))
            {
                if (directory.Parent == null)
                    throw new NullReferenceException("Parent Directory is null");

                StringBuilder zipNameBuilder = new StringBuilder();
                zipNameBuilder.Append(directory.Parent.FullName);
                zipNameBuilder.Append(directory.Name);
                zipNameBuilder.Append(".zip");
                zipFileFullName = zipNameBuilder.ToString();
            }

            //如果指定目标文件全名不包含zip后缀，则抛出异常
            if (zipFileFullName.Length > 0 && !zipFileFullName.EndsWith(".zip"))
                throw new NullReferenceException("The extension of file is error");

            FastZip fZip = new FastZip();

            if (password.Length > 0)
                fZip.Password = password;

            fZip.CreateZip(zipFileFullName, directory.FullName, isRecurse, fileFilter);
        }

        /// <summary>
        /// 将压缩文件解压
        /// </summary>
        /// <param name="file">针对FileInfo的扩展</param>
        /// <param name="targetDirectory">目标文件夹 没指定则压缩文件和被压缩文件在同一目录</param>
        /// <param name="password">压缩文件密码</param>
        /// <param name="fileFilter">解压文件类型过滤</param>
        public static void UnZip(this FileInfo file, string targetDirectory = "", 
            string password = "", string fileFilter = "")
        {
            //如果文件后缀不是.zip结尾则退出
            if (!file.FullName.ToLower().EndsWith(".zip"))
                throw new NullReferenceException("the sourece file is not zip file");

            //如果没有指定目标文件夹，则解压到和压缩文件同一目录下
            if (targetDirectory.Length == 0)
            {
                var dir = file.Directory;
                if (dir == null)
                    throw new NullReferenceException("Directory of file is null");
                targetDirectory = dir.FullName + "\\" + file.Name.Replace(file.Extension, ""); 
            }
          
            FastZip fZip = new FastZip();

            if (password.Length > 0)
                fZip.Password = password;

            fZip.ExtractZip(file.FullName, targetDirectory, fileFilter);
        }
    }
}
