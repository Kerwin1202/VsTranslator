using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using Translate.Core.Translator.Entities;
using Translate.Core.Translator.Enums;
using Translate.Core.Translator.Google.Entities;
using Translate.Core.Translator.Utils;

namespace Translate.Core.Translator.Google
{
    using static GoogleUtils;

    public class GoogleTranslator : ITranslator
    {

        private static readonly List<TranslationLanguage> TargetLanguages;
        private static readonly List<TranslationLanguage> SourceLanguages;


        static GoogleTranslator()
        {
            TargetLanguages = new List<TranslationLanguage>()
            {
                new TranslationLanguage ("af", "Afrikaans / 南非荷兰语"),
                new TranslationLanguage ("sq", "Albanian / 阿尔巴尼亚语"),
                new TranslationLanguage ("ar", "Arabic / 阿拉伯语"),
                new TranslationLanguage ("be", "Belarusian / 白俄罗斯语"),
                new TranslationLanguage ("bg", "Bulgarian / 保加利亚语"),
                new TranslationLanguage ("ca", "Catalan / 加泰罗尼亚语"),
                new TranslationLanguage ("zh-CN", "Chinese (Simplified) / 简体中文"),
                new TranslationLanguage ("zh-TW", "Chinese (Traditional) / 繁体中文"),
                new TranslationLanguage ("hr", "Croatian / 克罗地亚语"),
                new TranslationLanguage ("cs", "Czech / 捷克语"),
                new TranslationLanguage ("da", "Danish / 丹麦语"),
                new TranslationLanguage ("nl", "Dutch / 荷兰语"),
                new TranslationLanguage ("en", "English / 英语"),
                new TranslationLanguage ("et", "Estonian / 爱沙尼亚语"),
                new TranslationLanguage ("tl", "Filipino / 菲律宾语"),
                new TranslationLanguage ("fi", "Finnish / 芬兰语"),
                new TranslationLanguage ("fr", "French / 法语"),
                new TranslationLanguage ("gl", "Galician / 加利西亚语"),
                new TranslationLanguage ("de", "German / 德语"),
                new TranslationLanguage ("el", "Greek / 希腊语"),
                new TranslationLanguage ("iw", "Hebrew / 希伯来语"),
                new TranslationLanguage ("hi", "Hindi / 印地语"),
                new TranslationLanguage ("hu", "Hungarian / 匈牙利语"),
                new TranslationLanguage ("is", "Icelandic / 冰岛语"),
                new TranslationLanguage ("id", "Indonesian / 印度尼西亚语"),
                new TranslationLanguage ("ga", "Irish / 爱尔兰语"),
                new TranslationLanguage ("it", "Italian / 意大利语"),
                new TranslationLanguage ("ja", "Japanese / 日语"),
                new TranslationLanguage ("ko", "Korean / 朝鲜语"),
                new TranslationLanguage ("lv", "Latvian / 拉脱维亚语"),
                new TranslationLanguage ("lt", "Lithuanian / 立陶宛语"),
                new TranslationLanguage ("mk", "Macedonian / 马其顿语"),
                new TranslationLanguage ("ms", "Malay / 马来语"),
                new TranslationLanguage ("mt", "Maltese / 马耳他语"),
                new TranslationLanguage ("fa", "Persian / 波斯语"),
                new TranslationLanguage ("pl", "Polish / 波兰语"),
                new TranslationLanguage ("pt", "Portugese / 葡萄牙语"),
                new TranslationLanguage ("ro", "Romanian / 罗马尼亚语"),
                new TranslationLanguage ("ru", "Russian / 俄语"),
                new TranslationLanguage ("sr", "Serbian / 塞尔维亚语"),
                new TranslationLanguage ("sk", "Slovak / 斯洛伐克语"),
                new TranslationLanguage ("sl", "Slovenian / 斯洛文尼亚语"),
                new TranslationLanguage ("es", "Spanish / 西班牙语"),
                new TranslationLanguage ("sw", "Swahili / 斯瓦希里语"),
                new TranslationLanguage ("sv", "Swedish / 瑞典语"),
                new TranslationLanguage ("th", "Thai / 泰语"),
                new TranslationLanguage ("tr", "Turkish / 土耳其语"),
                new TranslationLanguage ("uk", "Ukranian / 乌克兰语"),
                new TranslationLanguage ("vi", "Vietnamese / 越南语"),
                new TranslationLanguage ("cy", "Welsh / 威尔士语"),
                new TranslationLanguage ("yi", "Yiddish / 意第绪语"),
                new TranslationLanguage ("mn", "Mongolian / 蒙古语"),
                new TranslationLanguage ("la", "Latin / 拉丁语")
            };

            SourceLanguages = new List<TranslationLanguage>() { new TranslationLanguage("auto", "Auto-detect / 自动检测") };
            SourceLanguages.AddRange(TargetLanguages);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="text">text's length must between 0 and 5000</param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        private GoogleTransResult TranslateByHttp(string text, string from = "en", string to = "zh-CN")
        {
            if (new Regex(@"[^\S\r\n]+").IsMatch(text))
            {
                return TranslateByMobile(text, from, to);
            }
            return TranslateByWebapp(text, from, to);
        }

        /// <summary>
        /// 旧版的
        /// </summary>
        /// <param name="text"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        private GoogleTransResult TranslateByWebapp(string text, string from = "en", string to = "zh-CN")
        {
            if (!(text.Length > 0 && text.Length < 5000))
            {
                return new GoogleTransResult()
                {
                    From = "Unknown",
                    TargetText = "Only 5000 letters!"
                };
            }
            text = text.Replace("\\", "");

            var tk = GetTk(text);

            var uri = $"https://translate.google.cn/translate_a/single?client=webapp&sl={from}&tl={to}&hl=zh-CN&dt=t&ie=UTF-8&oe=UTF-8&ssel=6&tsel=3&kc=0&tk={tk}&q={HttpUtility.UrlEncode(text)}";
            var html = string.Empty;
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(uri);
            using (var response = httpWebRequest.GetResponse())
            {
                using (Stream stream = response.GetResponseStream())
                {
                    if (stream == null)
                    {
                        return null;
                    }
                    using (var sr = new StreamReader(stream))
                    {
                        html = sr.ReadToEnd();
                    }
                }
            }

            if (html.Contains("/sorry/index?continue=") && html.Contains("302 Moved"))
            {
                return new GoogleTransResult()
                {
                    From = "Unknown",
                    TargetText = "Current IP traffic anomaly, can not be translated!"
                };
            }
            if (html.Contains("Error 403 (禁止访问)!"))
            {
                return new GoogleTransResult()
                {
                    From = "Unknown",
                    TargetText = "Error 403 (禁止访问)!"
                };
            }

            dynamic tempResult = Newtonsoft.Json.JsonConvert.DeserializeObject(html);
            var resarry = Newtonsoft.Json.JsonConvert.DeserializeObject(tempResult[0].ToString());
            var length = (resarry.Count);
            var str = new System.Text.StringBuilder();
            for (int i = 0; i < length; i++)
            {
                var res = Newtonsoft.Json.JsonConvert.DeserializeObject(resarry[i].ToString());
                str.Append(res[0].ToString());
            }
            return new GoogleTransResult()
            {
                From = tempResult[2].ToString(),
                TargetText = str.ToString()
            };
        }

        /// <summary>
        /// 手机版
        /// </summary>
        /// <param name="text"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        private GoogleTransResult TranslateByMobile(string text, string from = "en", string to = "zh-CN")
        {
            if (!(text.Length > 0 && text.Length < 5000))
            {
                return new GoogleTransResult()
                {
                    From = "Unknown",
                    TargetText = "Only 5000 letters!"
                };
            }
            var url = $"https://translate.google.cn/m?sl={from}&tl={to}&hl={to}&q={HttpUtility.UrlEncode(text)}";
            var html = string.Empty;
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.UserAgent = "Mozilla/5.0 (Linux; Android 10; GM1910) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/94.0.4606.85 Mobile Safari/537.36";
            using (var response = httpWebRequest.GetResponse())
            {
                using (Stream stream = response.GetResponseStream())
                {
                    if (stream == null)
                    {
                        return null;
                    }
                    using (var sr = new StreamReader(stream))
                    {
                        html = sr.ReadToEnd();
                    }
                }
            }

            var match = new Regex(@"<div[^>]*?class=""result-container""[^>]*>([\s\S]*?)<\/div>").Match(html);
            if (match.Success)
            {
                return new GoogleTransResult()
                {
                    From = from,
                    TargetText = new Regex(@"(<\/?[^>]+>)").Replace(match.Groups[1].Value, string.Empty)
                };
            }
            return new GoogleTransResult()
            {
                From = "Unknown",
                TargetText = "No Result!"
            };
        }

        public string GetIdentity()
        {
            return "Google";
        }
        public static string GetName()
        {
            return "Google Translator / 谷歌翻译";
        }
        public static string GetChineseLanguage()
        {
            return "zh-CN";
        }


        public static string GetDescription()
        {
            return "place the single request control within 5000 bytes in length (One Chinese Is a byte), you can on the website translation https://translate.google.cn/";
        }
        public static string GetWebsite()
        {
            return "https://translate.google.cn/";
        }

        public List<TranslationLanguage> GetAllTargetLanguages()
        {
            return TargetLanguages;
        }

        public List<TranslationLanguage> GetAllSourceLanguages()
        {
            return SourceLanguages;
        }

        public static List<TranslationLanguage> GetTargetLanguages()
        {
            return TargetLanguages;
        }

        public static List<TranslationLanguage> GetSourceLanguages()
        {
            return SourceLanguages;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text">text's length must between 0 and 5000</param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public TranslationResult Translate(string text, string @from = "en", string to = "zh-CN")
        {
            TranslationResult result = new TranslationResult()
            {
                SourceLanguage = @from,
                TargetLanguage = to,
                SourceText = text,
                TargetText = "",
                FailedReason = ""
            };
            if (SourceLanguages.Count(sl => sl.Code == @from) <= 0)
            {
                result.TranslationResultTypes = TranslationResultTypes.Failed;
                result.FailedReason = "unrecognizable source language";
            }
            else if (TargetLanguages.Count(tl => tl.Code == to) <= 0)
            {
                result.TranslationResultTypes = TranslationResultTypes.Failed;
                result.FailedReason = "unrecognizable target language";
            }
            else
            {
                try
                {
                    result.TranslationResultTypes = TranslationResultTypes.Successed;
                    GoogleTransResult googleTransResult = TranslateByHttp(text, from, to);
                    result.SourceLanguage = googleTransResult.From;
                    result.TargetText = googleTransResult.TargetText;
                }
                catch (Exception exception)
                {
                    result.FailedReason = exception.Message;
                    result.TranslationResultTypes = TranslationResultTypes.Failed;
                }
            }
            return result;
        }
    }
}