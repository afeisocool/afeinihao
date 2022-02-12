using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AfeiLib
{
    /// <summary>
    /// 字符串辅助
    /// </summary>
    public static class StringHelper
    {
        /// <summary>
        /// 把对象转化成json字串
        /// </summary>
        /// <param name="target">objec</param>
        /// <returns></returns>
        public static string Tojson(this System.Object target)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(target);
        }
        /// <summary>
        /// 秘钥
        /// </summary>
        public static string key = "afeiafei";

        /// <summary>
        /// md5加密
        /// </summary>
        /// <param name="target">string</param>
        public static string Md5_Encrypt(this System.String str)
        {
            if (str == "")
            {
                return "";
            }
            StringBuilder ret = new StringBuilder();
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] inputByteArray = Encoding.Default.GetBytes(str);
            des.Key = ASCIIEncoding.ASCII.GetBytes(key);
            des.IV = ASCIIEncoding.ASCII.GetBytes(key);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            ret = new StringBuilder();
            foreach (byte b in ms.ToArray())
            {
                ret.AppendFormat("{0:X2}", b);
            }
            return ret.ToString();
        }
        /// <summary>
        /// md5加密
        /// </summary>
        /// <param name="str">string</param>
        /// <param name="keys">keys</param>
        /// <returns></returns>
        public static string Md5_Encrypt(this System.String str, string keys)
        {
            if (str == "")
            {
                return "";
            }
            StringBuilder ret = new StringBuilder();
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] inputByteArray = Encoding.Default.GetBytes(str);
            des.Key = ASCIIEncoding.ASCII.GetBytes(keys);
            des.IV = ASCIIEncoding.ASCII.GetBytes(keys);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            ret = new StringBuilder();
            foreach (byte b in ms.ToArray())
            {
                ret.AppendFormat("{0:X2}", b);
            }
            return ret.ToString();
        }
        /// <summary>
        /// md5解密
        /// </summary>
        /// <param name="str">string</param>
        /// <returns></returns>
        public static string Md5_Decrypt(this System.String str)
        {
            if (str == "")
            {
                return "";
            }
            StringBuilder ret = new StringBuilder();
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] inputByteArray = new byte[str.Length / 2];
            for (int x = 0; x < str.Length / 2; x++)
            {
                int i = (Convert.ToInt32(str.Substring(x * 2, 2), 16));
                inputByteArray[x] = (byte)i;
            }
            des.Key = ASCIIEncoding.ASCII.GetBytes(key);
            des.IV = ASCIIEncoding.ASCII.GetBytes(key);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            return System.Text.Encoding.Default.GetString(ms.ToArray());
        }
        /// <summary>
        /// md5解密
        /// </summary>
        /// <param name="str">string</param>
        /// <param name="keys">keys</param>
        /// <returns></returns>
        public static string Md5_Decrypt(this System.String str, string keys)
        {
            if (str == "")
            {
                return "";
            }
            StringBuilder ret = new StringBuilder();
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] inputByteArray = new byte[str.Length / 2];
            for (int x = 0; x < str.Length / 2; x++)
            {
                int i = (Convert.ToInt32(str.Substring(x * 2, 2), 16));
                inputByteArray[x] = (byte)i;
            }
            des.Key = ASCIIEncoding.ASCII.GetBytes(keys);
            des.IV = ASCIIEncoding.ASCII.GetBytes(keys);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            return System.Text.Encoding.Default.GetString(ms.ToArray());
        }
        /// <summary>
        /// 字符串反序列化成T对象
        /// </summary>
        /// <typeparam name="T">对象</typeparam>
        /// <param name="str">string</param>
        /// <returns></returns>
        public static T ToObject<T>(this System.String str) where T : class, new()
        {
            return JsonConvert.DeserializeObject<T>(str);
        }
    }
}
