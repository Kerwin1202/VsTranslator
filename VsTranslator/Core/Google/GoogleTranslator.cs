using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Web;
using Microsoft.VisualStudio.Shell.Interop;
using VsTranslator.Core.Utils;

namespace VsTranslator.Core.Google
{
    public class GoogleTranslator
    {
        public string Translate(string text)
        {
            string baseUrl = "http://translate.google.com/translate_a/t";
            string data = CreateQuerystring(new Dictionary<string, string>()
            {
                {"client","f"},
                {"otf", "1"},
                {"pc", "0"},
                {"sl", "en"},
                {"tl", "zh-CN"},
                {"hl", "zh-CN"},
                {"text", text}
            });

            string response = GetHttpResponse(baseUrl, data);

            return response;
        }

        public static string GetHttpResponse(String url, String data)
        {
            var html = new HttpHelper().GetHtml(new HttpItem()
            {
                Url = url,
                Postdata = data,
                Method = "POST",
                ContentType = "application/x-www-form-urlencoded;charset=utf-8",
                UserAgent = "User-Agent: Mozilla/5.0"
            }).Html;

            return html;
        }

        public static string CreateQuerystring(Dictionary<string, string> args)
        {
            StringBuilder sb = new StringBuilder();
            foreach (string name in args.Keys)
            {
                sb.Append(HttpUtility.UrlEncode(name));
                sb.Append("=");
                sb.Append(HttpUtility.UrlEncode(args[name]));
                sb.Append("&");
            }
            return sb.ToString(0, Math.Max(sb.Length - 1, 0));
        }
    }
}