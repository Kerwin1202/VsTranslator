using System.Security.Cryptography;
using System.Text;

namespace VsTranslator.Core.Translator.Utils
{
    public static class Encrypts
    {
        /// <summary>
        /// 创建md5
        /// </summary>
        /// <param name="inputString">原字符串</param>
        /// <param name="md5Encoding">编码格式</param>
        /// <returns></returns>
        public static string CreateMd5EncryptFromString(string inputString, Encoding md5Encoding)
        {
            var buffer = md5Encoding.GetBytes(inputString);
            var md5StringBuilder = new StringBuilder();
            using (var md5 = MD5.Create())
            {
                var resultBuffer = md5.ComputeHash(buffer, 0, buffer
                      .Length);
                foreach (var b in resultBuffer)
                {
                    md5StringBuilder.AppendFormat("{0:x2}", b);
                }
                return md5StringBuilder.ToString();
            }
        }
    }
}