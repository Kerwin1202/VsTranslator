using System;
using System.Text;

namespace Translate.Core.Translator.Google
{
    /// <summary>
    /// 版权声明：本文为博主原创文章，遵循 CC 4.0 BY-SA 版权协议，转载请附上原文出处链接和本声明。
    /// 本文链接：https://blog.csdn.net/u013070165/article/details/85096834
    /// </summary>
    internal class GoogleUtils
    {

        internal static string GetTk(string a, string TKK = "406398.2087938574")
        {
            string[] e = TKK.Split('.');
            int d = 0;
            int h = 0;
            h = Number(e[0]);
            byte[] g0 = Encoding.UTF8.GetBytes(a);
            long aa = h;
            for (d = 0; d < g0.Length; d++)
            {
                aa += g0[d];
                aa = Convert.ToInt64(B(aa, "+-a^+6"));
            }
            aa = Convert.ToInt64(B(aa, "+-3^+b+-f"));
            long bb = aa ^ Number(e[1]);
            aa = bb;
            aa = aa + bb;
            bb = aa - bb;
            aa = aa - bb;
            if (0 > aa)
            {
                aa = (aa & 2147483647) + 2147483648;
            }
            aa %= (long)1e6;
            return aa.ToString() + "." + (aa ^ h);
        }
        internal static string B(long a, string b)
        {
            for (int d = 0; d < b.Length - 2; d += 3)
            {
                char c = CharAt(b, d + 2);
                int c0 = 'a' <= c ? CharCodeAt(c, 0) - 87 : Number(c);
                long c1 = '+' == CharAt(b, d + 1) ? a >> c0 : a << c0;
                a = '+' == CharAt(b, d) ? a + c1 & 4294967295 : a ^ c1;
            }
            a = Number(a);
            return a.ToString();
        }

        //实现js的charAt方法
        internal static char CharAt(object obj, int index)
        {
            char[] chars = obj.ToString().ToCharArray();
            return chars[index];
        }
        //实现js的charCodeAt方法
        internal static int CharCodeAt(object obj, int index)
        {
            char[] chars = obj.ToString().ToCharArray();
            return (int)chars[index];
        }

        //实现js的Number方法
        internal static int Number(object cc)
        {
            try
            {
                long a = Convert.ToInt64(cc.ToString());
                int b = a > 2147483647 ? (int)(a - 4294967296) : a < -2147483647 ? (int)(a + 4294967296) : (int)a;
                return b;
            }
            catch (Exception)
            {
                return 0;
            }
        }
    }
}