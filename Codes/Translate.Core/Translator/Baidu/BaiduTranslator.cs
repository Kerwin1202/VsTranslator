using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web;
using Translate.Core.Translator.Baidu.Entities;
using Translate.Core.Translator.Entities;
using Translate.Core.Translator.Enums;
using Translate.Core.Translator.Utils;
using WebException = System.Net.WebException;

namespace Translate.Core.Translator.Baidu
{
    public class BaiduTranslator : ITranslator
    {
        private BaiduTranslator()
        {

        }

        static BaiduTranslator()
        {

            TargetLanguages = new List<TranslationLanguage>()
            {
                new TranslationLanguage("zh","Chinese (Simplified) / 简体中文"),
                new TranslationLanguage("cht","Chinese (Traditional) / 繁体中文"),
                new TranslationLanguage("en","English / 英语"),
                new TranslationLanguage("yue","Cantonese / 粤语"),
                new TranslationLanguage("wyw","Classical Chinese / 文言文"),
                new TranslationLanguage("jp","Japanese / 日语"),
                new TranslationLanguage("kor","Korean / 韩语"),
                new TranslationLanguage("fra","French / 法语"),
                new TranslationLanguage("spa","Spanish / 西班牙语"),
                new TranslationLanguage("th","Thai / 泰语"),
                new TranslationLanguage("ara","Arabic / 阿拉伯语"),
                new TranslationLanguage("ru","Russian / 俄语"),
                new TranslationLanguage("pt","Portuguese / 葡萄牙语"),
                new TranslationLanguage("de","German / 德语"),
                new TranslationLanguage("it","Italian / 意大利语"),
                new TranslationLanguage("el","Greek / 希腊语"),
                new TranslationLanguage("nl","Dutch / 荷兰语"),
                new TranslationLanguage("pl","Polish / 波兰语"),
                new TranslationLanguage("bul","Bulgarian / 保加利亚语"),
                new TranslationLanguage("est","Estonian / 爱沙尼亚语"),
                new TranslationLanguage("dan","Danish / 丹麦语"),
                new TranslationLanguage("fin","Finnish / 芬兰语"),
                new TranslationLanguage("cs","Czech / 捷克语"),
                new TranslationLanguage("rom","Romanian / 罗马尼亚语"),
                new TranslationLanguage("slo","Slovenian / 斯洛文尼亚语"),
                new TranslationLanguage("swe","Swedish / 瑞典语"),
                new TranslationLanguage("hu","Hungarian / 匈牙利语"),
                new TranslationLanguage("vie","Vietnamese / 越南语")
            };
            SourceLanguages = new List<TranslationLanguage>() { new TranslationLanguage("auto", "Auto-detect / 自动检测") };
            SourceLanguages.AddRange(TargetLanguages);
        }

        private readonly string TranslateUrl = "http://api.fanyi.baidu.com/api/trans/vip/translate";
        //Get appid and client secret from http://api.fanyi.baidu.com/api/trans/product/index
        private readonly string _appid;
        private readonly string _clientSecret;

        private static readonly List<TranslationLanguage> TargetLanguages;
        private static readonly List<TranslationLanguage> SourceLanguages;


        public BaiduTranslator(string appid, string clientSecret)
        {
            if (string.IsNullOrWhiteSpace(appid) || string.IsNullOrWhiteSpace(clientSecret))
            {
                throw new Exception("app id and client secret is necessary");
            }
            _appid = appid;
            _clientSecret = clientSecret;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="text">text's length between 0 and 2000</param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        private BaiduTransResult TranslateByHttp(string text, string from = "en", string to = "zh")
        {
            if (!(text.Length > 0 && text.Length < 2000))
            {
                return null;
            }
            try
            {
                var timestamp = Times.TimeStampWithMsec;
                string uri = $"{TranslateUrl}?q={ HttpUtility.UrlEncode(text)}&from={from}&to={to}&appid={_appid}&salt={timestamp}&sign={Encrypts.CreateMd5EncryptFromString($"{_appid}{text}{timestamp}{_clientSecret}", Encoding.UTF8)}";
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(uri);
                using (var response = httpWebRequest.GetResponse())
                {
                    using (Stream stream = response.GetResponseStream())
                    {
                        if (stream == null)
                        {
                            return null;
                        }
                        DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(BaiduTransResult));
                        //Get deserialized object from JSON stream
                        BaiduTransResult baiduTransResult = (BaiduTransResult)serializer.ReadObject(stream);
                        try
                        {
                            Console.WriteLine("Baidu translation for source text '{0}' from {1} to {2} is", text, from, to);
                            Console.WriteLine(baiduTransResult.TransResult[0].Dst);
                        }
                        catch (Exception)
                        {
                            throw new Exception("翻译失败. 可能原因有：百度的免费版和高级版的QPS分别为 1/s 10/s。 具体：http://api.fanyi.baidu.com/api/trans/product/identify 2020-02-06 ");
                        }
                        return baiduTransResult;
                    }
                }
            }
            catch (WebException e)
            {
                Utils.WebException.ProcessWebException(e);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
            return null;
        }

        public string GetIdentity()
        {
            return "Baidu";
        }

        public static string GetName()
        {
            return "Baidu Translator / 百度翻译";
        }

        public static string GetDescription()
        {
            return "1. If the translation of the month number of characters is less than 2 million, that month free if more than 2 million characters, in accordance with 49 Yuan/million characters to pay monthly fee for total number of translated characters, you can on the website translation http://fanyi.baidu.com/ \r\n2.In order to guarantee the quality of translation, place the single request control within 6000 bytes in length. (About 2000 Chinese characters)";
            //1. 若当月翻译字符数≤2百万，当月免费；若超过2百万字符，按照49元/百万字符支付当月全部翻译字符数费用， 你也可以在网站上翻译 http://fanyi.baidu.com/ \r\n2. 为保证翻译质量，请将单次请求长度控制在 6000 bytes以内。（汉字约为2000个）
        }

        public static string GetWebsite()
        {
            return "http://fanyi.baidu.com/";
        }

        public static string GetChineseLanguage()
        {
            return "zh";
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
        /// <param name="text">text's length between 0 and 2000</param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public TranslationResult Translate(string text, string @from = "en", string to = "zh")
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
                    BaiduTransResult baiduTransResult = TranslateByHttp(text, from, to);
                    if (baiduTransResult == null)
                    {
                        result.FailedReason = "Translate Failed!";
                        result.TranslationResultTypes = TranslationResultTypes.Failed;
                    }
                    else
                    {
                        foreach (var transResult in baiduTransResult.TransResult)
                        {
                            result.TargetText += transResult.Dst + "\r\n";
                        }
                        if (result.TargetText.Length > 0)
                        {
                            result.TargetText = result.TargetText.Substring(0, result.TargetText.Length - 2);
                        }
                        result.TranslationResultTypes = TranslationResultTypes.Successed;
                        result.SourceLanguage = baiduTransResult.From;
                        result.TargetLanguage = baiduTransResult.To;
                    }
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