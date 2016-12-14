using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using Microsoft.VisualStudio.Shell.Interop;
using VsTranslator.Core.Utils;

namespace VsTranslator.Core.Google
{
    public class GoogleTranslator
    {
        private readonly CSharpRunJavascript _javascript = new CSharpRunJavascript();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text">text's length must between 0 and 5000</param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public string Translate(string text, string from = "en", string to = "zh-CN")
        {
            if (!(text.Length > 0 && text.Length < 5000))
            {
                return string.Empty;
            }
            var tk = GetTk(text);

            var result = new HttpHelper().GetHtml(new HttpItem()
            {
                Url =
                    "http://translate.google.cn/translate_a/single?client=t&sl=zh-CN&tl=en&hl=zh-CN&dt=t&ie=UTF-8&oe=UTF-8&ssel=6&tsel=3&kc=0&tk=" + tk + "&q=" + text
            }).Html;
            //
            return new Regex("\\[\\[\\[\"(.+?)\",\"(.+?)\",,,.+?\\]\\],,\".+?\"\\]").Match(result).Groups[1].Value;

            //[[["You have a good day today","你今天过得好不好",,,3]],,"zh-CN"]
            //[[["你好”","hello\"",,,1]],,"en"]
            //[[["你好","hello",,,1]],,"en"]
        }


        private string GetTk(string text)
        {
            return _javascript.Eval("((function() {var TKK = ((function() {var a = 561666268;var b = 1526272306;return 406398 + '.' + (a + b);  })()); function b(a, b) {for (var d = 0; d < b.length - 2; d += 3) {        var c = b.charAt(d + 2),            c = 'a' <= c ? c.charCodeAt(0) - 87 : Number(c),            c = '+' == b.charAt(d + 1) ? a >>> c : a << c;        a = '+' == b.charAt(d) ? a + c & 4294967295 : a ^ c    }    return a  }    function tk(a) {      for (var e = TKK.split('.'), h = Number(e[0]) || 0, g = [], d = 0, f = 0; f < a.length; f++) {          var c = a.charCodeAt(f);          128 > c ? g[d++] = c : (2048 > c ? g[d++] = c >> 6 | 192 : (55296 == (c & 64512) && f + 1 < a.length && 56320 == (a.charCodeAt(f + 1) & 64512) ? (c = 65536 + ((c & 1023) << 10) + (a.charCodeAt(++f) & 1023), g[d++] = c >> 18 | 240, g[d++] = c >> 12 & 63 | 128) : g[d++] = c >> 12 | 224, g[d++] = c >> 6 & 63 | 128), g[d++] = c & 63 | 128)      }      a = h;      for (d = 0; d < g.length; d++) a += g[d], a = b(a, '+-a^+6');      a = b(a, '+-3^+b+-f');      a ^= Number(e[1]) || 0;      0 > a && (a = (a & 2147483647) + 2147483648);      a %= 1E6;      return a.toString() + '.' + (a ^ h)  }  return tk('" + text + "'); })())").ToString();
        }

    }
}