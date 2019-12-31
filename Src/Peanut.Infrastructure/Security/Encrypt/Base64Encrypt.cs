/*********************************************************************** 
 * 项目名称 :  Peanut.Helper   
 * 项目描述 :      
 * 类 名 称 :  Base64Encrypt 
 * 说    明 :      
 * 作    者 :  XHT  
 * 创建时间 :  2018/12/08 14:55:42
 * 更新时间 :  2018/12/08 14:55:42 
************************************************************************ 
 * Copyright @   2018. All rights reserved. 
************************************************************************/

using System;
using System.Collections;
using System.Text;
using System.Text.RegularExpressions;


namespace Peanut.Infrastructure.Encrypt
{
    /// <summary>
    /// Base64加密解密
    /// 
    /// 目的：
    ///     提供Base64加密解密方法
    /// 
    /// 使用规范：
    ///     略
    /// </summary>
    public sealed class Base64Encrypt
    {
        /// <summary>
        /// Base64加密
        /// </summary>
        /// <param name="inputString">要加密的字符串</param>
        /// <returns></returns>
        public static string Base64Encode(string inputString)
        {
            if (string.IsNullOrWhiteSpace(inputString))
                throw new ArgumentNullException();

            char[] Base64Code =
                {'A','B','C','D','E','F','G','H','I','J','K','L','M','N','O','P','Q','R','S','T',
                'U','V','W','X','Y','Z','a','b','c','d','e','f','g','h','i','j','k','l','m','n',
                'o','p','q','r','s','t','u','v','w','x','y','z','0','1','2','3','4','5','6','7',
                '8','9','+','/','='};

            const byte empty = (byte)0;
            ArrayList byteMessage = new ArrayList(Encoding.Default.GetBytes(inputString));

            int messageLen = byteMessage.Count;
            int page = messageLen / 3;
            int use;

            if ((use = messageLen % 3) > 0)
            {
                for (int i = 0; i < 3 - use; i++)
                    byteMessage.Add(empty);

                page++;
            }

            StringBuilder outmessage = new StringBuilder(page * 4);
            for (int i = 0; i < page; i++)
            {
                byte[] instr = new byte[3];
                instr[0] = (byte)byteMessage[i * 3];
                instr[1] = (byte)byteMessage[i * 3 + 1];
                instr[2] = (byte)byteMessage[i * 3 + 2];
                int[] outstr = new int[4];
                outstr[0] = instr[0] >> 2;
                outstr[1] = ((instr[0] & 0x03) << 4) ^ (instr[1] >> 4);

                if (!instr[1].Equals(empty))
                    outstr[2] = ((instr[1] & 0x0f) << 2) ^ (instr[2] >> 6);
                else
                    outstr[2] = 64;
                if (!instr[2].Equals(empty))
                    outstr[3] = (instr[2] & 0x3f);
                else
                    outstr[3] = 64;

                outmessage.Append(Base64Code[outstr[0]]);
                outmessage.Append(Base64Code[outstr[1]]);
                outmessage.Append(Base64Code[outstr[2]]);
                outmessage.Append(Base64Code[outstr[3]]);
            }

            return outmessage.ToString();
        }

        /// <summary>
        /// Base64解密
        /// </summary>
        /// <param name="inputString">要解密的字符串</param>
        public static string Base64Decode(string inputString)
        {
            if (string.IsNullOrWhiteSpace(inputString))
                throw new ArgumentNullException();

            inputString = inputString.Replace(" ", "+");

            if ((inputString.Length % 4) != 0)
                return "包含不正确的BASE64编码";

            if (!Regex.IsMatch(inputString, "^[A-Z0-9/+=]*$", RegexOptions.IgnoreCase))
                return "包含不正确的BASE64编码";

            const string Base64Code = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=";
            int page = inputString.Length / 4;
            ArrayList outMessage = new ArrayList(page * 3);
            char[] message = inputString.ToCharArray();

            for (int i = 0; i < page; i++)
            {
                byte[] instr = new byte[4];
                instr[0] = (byte)Base64Code.IndexOf(message[i * 4]);
                instr[1] = (byte)Base64Code.IndexOf(message[i * 4 + 1]);
                instr[2] = (byte)Base64Code.IndexOf(message[i * 4 + 2]);
                instr[3] = (byte)Base64Code.IndexOf(message[i * 4 + 3]);
                byte[] outstr = new byte[3];
                outstr[0] = (byte)((instr[0] << 2) ^ ((instr[1] & 0x30) >> 4));

                if (instr[2] != 64)
                    outstr[1] = (byte)((instr[1] << 4) ^ ((instr[2] & 0x3c) >> 2));
                else
                    outstr[2] = 0;

                if (instr[3] != 64)
                    outstr[2] = (byte)((instr[2] << 6) ^ instr[3]);
                else
                    outstr[2] = 0;

                outMessage.Add(outstr[0]);

                if (outstr[1] != 0)
                    outMessage.Add(outstr[1]);

                if (outstr[2] != 0)
                    outMessage.Add(outstr[2]);
            }

            var tmp = Type.GetType("System.Byte");

            if (tmp == null)
                return string.Empty;

            byte[] outbyte = (byte[])outMessage.ToArray(tmp);

            return Encoding.Default.GetString(outbyte);
        }
    }
}
