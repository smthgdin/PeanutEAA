/*********************************************************************** 
 * 项目名称 :  Peanut.Helper   
 * 项目描述 :      
 * 类 名 称 :  DeepCloneHelper 
 * 说    明 :      
 * 作    者 :  XHT  
 * 创建时间 :  2018/12/08 16:06:21
 * 更新时间 :  2018/12/08 16:06:21
************************************************************************ 
 * Copyright @   2018. All rights reserved. 
************************************************************************/

using System;
using System.IO;
using System.Security.Cryptography;


namespace Peanut.Infrastructure.Encrypt
{
    /// <summary>
    /// DES加密解密
    /// 
    /// 目的：
    ///     提供DES加密解密方法
    /// 
    /// 使用规范：
    ///     略
    /// </summary>
    public sealed class DESEncrypt
    {
        //密钥
        private static readonly byte[] arrDESKey = { 42, 16, 93, 156, 78, 4, 218, 32 };
        private static readonly byte[] arrDESIV = { 55, 103, 246, 79, 36, 99, 167, 3 };

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="inputString">需要加密的字符串</param>
        /// <returns></returns>
        public static string DESEncode(string inputString)
        {
            if (string.IsNullOrWhiteSpace(inputString))
                throw new ArgumentNullException();

            DESCryptoServiceProvider objDES = new DESCryptoServiceProvider();
            MemoryStream objMemoryStream = new MemoryStream();
            
            CryptoStream objCryptoStream = new CryptoStream(objMemoryStream, objDES.CreateEncryptor(arrDESKey, arrDESIV), CryptoStreamMode.Write);
            StreamWriter objStreamWriter = new StreamWriter(objCryptoStream);

            objStreamWriter.Write(inputString);
            objStreamWriter.Flush();
            objCryptoStream.FlushFinalBlock();
            objMemoryStream.Flush();

            return Convert.ToBase64String(objMemoryStream.GetBuffer(), 0, (int)objMemoryStream.Length);
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="inputString">需要解密的字符串</param>
        /// <returns></returns>
        public static string DESDecode(string inputString)
        {
            if (string.IsNullOrWhiteSpace(inputString))
                throw new ArgumentNullException();

            DESCryptoServiceProvider objDES = new DESCryptoServiceProvider();

            byte[] arrInput = Convert.FromBase64String(inputString);
            MemoryStream memoryStream = new MemoryStream(arrInput);
            CryptoStream cryptoStream = new CryptoStream(memoryStream, objDES.CreateDecryptor(arrDESKey, arrDESIV), CryptoStreamMode.Read);

            StreamReader sReader = new StreamReader(cryptoStream);
            string rlt = sReader.ReadToEnd();

            sReader.Close();
            cryptoStream.Close();
            memoryStream.Close();

            return rlt;
        }

    }
}
