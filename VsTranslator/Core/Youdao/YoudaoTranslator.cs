using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web;
using VsTranslator.Core.Baidu.Entities;
using VsTranslator.Core.Entities;
using VsTranslator.Core.Enums;
using VsTranslator.Core.Utils;
using VsTranslator.Core.Youdao.Entities;
using VsTranslator.Core.Youdao.Enums;
using WebException = VsTranslator.Core.Utils.WebException;

namespace VsTranslator.Core.Youdao
{
    public class YoudaoTranslator : ITranslator
    {
        private YoudaoTranslator()
        {

        }

        private readonly List<TranslationLanguage> _targetLanguages;
        private readonly List<TranslationLanguage> _sourceLanguages;

        private const string TranslateUrl = "http://fanyi.youdao.com/openapi.do";

        //Get appid and client secret from http://fanyi.youdao.com/openapi
        private readonly string _appid;

        private readonly string _clientSecret;

        private const string Chinese = "ZH_CN";
        private const string Auto = "Auto";

        public YoudaoTranslator(string appid, string clientSecret)
        {
            _appid = appid;
            _clientSecret = clientSecret;

            _targetLanguages = new List<TranslationLanguage>()
            {
                new TranslationLanguage(Chinese, "Chinese (Simplified) / 简体中文"),
                new TranslationLanguage("EN", "English / 英语"),
                new TranslationLanguage("JA", "Japanese / 日语"),
                new TranslationLanguage("KR", "Korean / 韩语"),
                new TranslationLanguage("FR", "French / 法语"),
                new TranslationLanguage("RU", "Russian / 俄语"),
                new TranslationLanguage("SP", "Spanish / 西班牙语"),
                new TranslationLanguage("PT", "Portuguese / 葡萄牙语")
            };
            _sourceLanguages = new List<TranslationLanguage>() { new TranslationLanguage(Auto, "Auto-detect / 自动检测") };
            _sourceLanguages.AddRange(_targetLanguages);
        }

        /// <summary>
        /// Dictionaries only support translation between Chinese and English in translation results supported Britain, Japan and South Korea, France, Russia and the West to Chinese translation and Chinese to English translation
        /// about Q&A http://fanyi.youdao.com/openapi?path=faq
        /// about document http://fanyi.youdao.com/openapi?path=data-mode
        /// </summary>
        /// <param name="text">text's length must between 0 and 200</param>
        /// <param name="translateType"></param>
        private YoudaoTransResult GetTranslate(string text, TranslateType translateType = TranslateType.Dict | TranslateType.Translate)
        {
            if (!(text.Length > 0 && text.Length < 200))
            {
                return null;
            }
            try
            {
                string uri = $"{TranslateUrl}?keyfrom={_appid}&key={_clientSecret}&type=data&doctype=json&version=1.1&q={HttpUtility.UrlEncode(text)}";
                if (translateType != (TranslateType.Dict | TranslateType.Translate))
                {
                    uri += "&only=" + translateType.ToString().ToLower();
                }
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(uri);
                WebResponse response = null;
                try
                {
                    response = httpWebRequest.GetResponse();
                    using (Stream stream = response.GetResponseStream())
                    {
                        if (stream == null)
                        {
                            return null;
                        }
                        DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(YoudaoTransResult));
                        //Get deserialized object from JSON stream
                        YoudaoTransResult youdaoTransResult = (YoudaoTransResult)serializer.ReadObject(stream);
                        return youdaoTransResult;
                    }
                }
                catch
                {
                    throw;
                }
                finally
                {
                    if (response != null)
                    {
                        response.Close();
                        response = null;
                    }
                }
            }
            catch (System.Net.WebException e)
            {
                VsTranslator.Core.Utils.WebException.ProcessWebException(e);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }

        public string GetName()
        {
            return "Youdao Translator / 有道翻译";
        }

        public string GetDescription()
        {
            return "1. text's length must between 0 and 200 \r\n2. 1000 free access per hour, over visits will be conducted in ban suspended, will resume after 1 hour.\r\n3. Free daily flow of 100,000 characters, more than RMB 50 parts per million characters; for example, a cumulative translation 600,000 characters, you will need to charge RMB 25, you can on the website translation http://fanyi.youdao.com/";
            //1. 每小时1000次免费访问，超过访问次数后会进行封禁暂停服务，1小时后会自然恢复。\r\n2. 每日免费10万字符流量，超过部分每百万字符50元人民币；例如，一天累积翻译60万字符，则需要收费25元人民币
        }
        public string GetWebsite()
        {
            return "http://fanyi.youdao.com/";
        }

        public List<TranslationLanguage> GetTargetLanguages()
        {
            return _targetLanguages;
        }

        public List<TranslationLanguage> GetSourceLanguages()
        {
            return _sourceLanguages;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="text">text's length must between 0 and 200</param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public TranslationResult Translate(string text, string @from, string to)
        {
            TranslationResult result = new TranslationResult()
            {
                SourceLanguage = @from,
                TargetLanguage = to,
                SourceText = text,
                TargetText = "",
                FailedReason = ""
            };
            if (_sourceLanguages.Count(sl => sl.Code == @from) <= 0)
            {
                result.TranslationResultTypes = TranslationResultTypes.Failed;
                result.FailedReason = "unrecognizable source language";
            }
            else if (_targetLanguages.Count(tl => tl.Code == to) <= 0)
            {
                result.TranslationResultTypes = TranslationResultTypes.Failed;
                result.FailedReason = "unrecognizable target language";
            }
            else if (@from != Chinese && to != Chinese)
            {
                result.TranslationResultTypes = TranslationResultTypes.Failed;
                result.FailedReason = "Dictionaries only support translation between Chinese and English in translation results supported Britain, Japan and South Korea, France, Russia and the West to Chinese translation and Chinese to English translation";
            }
            else
            {
                try
                {
                    YoudaoTransResult youdaoTransResult = GetTranslate(text);
                    if (youdaoTransResult == null || youdaoTransResult.ErrorCode != ErrorCodes.Normal)
                    {
                        result.TranslationResultTypes = TranslationResultTypes.Failed;
                        result.FailedReason = "translate failed";
                    }
                    else
                    {
                        result.TranslationResultTypes = TranslationResultTypes.Successed;
                        result.TargetText = youdaoTransResult.Translation[0];
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