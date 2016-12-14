using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ConsoleApplication3
{
    class Program
    {
        static void Main(string[] args)
        {
            //eval('((function(){var a\x3d2620793759;var b\x3d-896917828;return 411590+\x27.\x27+(a+b)})())')
            CSharpRunJavascript js = new CSharpRunJavascript();
            string st = "你今天过得好不好";
            var tk = js.Eval("((function() {var TKK = ((function() {var a = 561666268;var b = 1526272306;return 406398 + '.' + (a + b);  })()); function b(a, b) {for (var d = 0; d < b.length - 2; d += 3) {        var c = b.charAt(d + 2),            c = 'a' <= c ? c.charCodeAt(0) - 87 : Number(c),            c = '+' == b.charAt(d + 1) ? a >>> c : a << c;        a = '+' == b.charAt(d) ? a + c & 4294967295 : a ^ c    }    return a  }    function tk(a) {      for (var e = TKK.split('.'), h = Number(e[0]) || 0, g = [], d = 0, f = 0; f < a.length; f++) {          var c = a.charCodeAt(f);          128 > c ? g[d++] = c : (2048 > c ? g[d++] = c >> 6 | 192 : (55296 == (c & 64512) && f + 1 < a.length && 56320 == (a.charCodeAt(f + 1) & 64512) ? (c = 65536 + ((c & 1023) << 10) + (a.charCodeAt(++f) & 1023), g[d++] = c >> 18 | 240, g[d++] = c >> 12 & 63 | 128) : g[d++] = c >> 12 | 224, g[d++] = c >> 6 & 63 | 128), g[d++] = c & 63 | 128)      }      a = h;      for (d = 0; d < g.length; d++) a += g[d], a = b(a, '+-a^+6');      a = b(a, '+-3^+b+-f');      a ^= Number(e[1]) || 0;      0 > a && (a = (a & 2147483647) + 2147483648);      a %= 1E6;      return a.toString() + '.' + (a ^ h)  }  return tk('" + st + "'); })())");
            Console.WriteLine(tk);

            Console.WriteLine(new HttpHelper().GetHtml(new HttpItem()
            {
                Url = "http://translate.google.cn/translate_a/single?client=t&sl=zh-CN&tl=en&hl=zh-CN&dt=t&ie=UTF-8&oe=UTF-8&ssel=6&tsel=3&kc=0&tk=" + tk + "&q=" + st
            }).Html);
            Console.ReadKey(true);
        }
    }
}
