using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web;
using Translate.Core.Translator.Entities;
using Translate.Core.Translator.Enums;
using Translate.Core.Translator.Youdao.Entities;
using Translate.Core.Translator.Youdao.Enums;

namespace Translate.Core.Translator.Youdao
{
    public class YoudaoTranslator : ITranslator
    {
        static YoudaoTranslator()
        {
            TargetLanguages = new List<TranslationLanguage>()
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
            SourceLanguages = new List<TranslationLanguage>() { new TranslationLanguage(Auto, "Auto-detect / 自动检测") };
            SourceLanguages.AddRange(TargetLanguages);
        }

        private static readonly List<TranslationLanguage> TargetLanguages;
        private static readonly List<TranslationLanguage> SourceLanguages;

        private const string TranslateUrl = "http://fanyi.youdao.com/openapi.do";

        //Get appid and client secret from http://fanyi.youdao.com/openapi
        private readonly string _appid;

        private readonly string _clientSecret;

        private const string Chinese = "ZH_CN";
        private const string English = "EN";
        private const string Auto = "Auto";

        /// <summary>
        /// use TranslateByHttp appid and client secret is necessary
        /// </summary>
        /// <param name="appid"></param>
        /// <param name="clientSecret"></param>
        public YoudaoTranslator(string appid, string clientSecret)
        {
            _appid = appid;
            _clientSecret = clientSecret;
        }

        /// <summary>
        /// use TranslateByPost
        /// </summary>
        public YoudaoTranslator()
        {

        }

        /// <summary>
        /// Dictionaries only support translation between Chinese and English in translation results supported Britain, Japan and South Korea, France, Russia and the West to Chinese translation and Chinese to English translation
        /// about Q&A http://fanyi.youdao.com/openapi?path=faq
        /// about document http://fanyi.youdao.com/openapi?path=data-mode
        /// </summary>
        /// <param name="text">text's length must between 0 and 200</param>
        /// <param name="translateType"></param>
        private YoudaoTransResult TranslateByHttp(string text, TranslateType translateType = TranslateType.Dict | TranslateType.Translate)
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
                    response?.Close();
                }
            }
            catch (WebException e)
            {
                Utils.WebException.ProcessWebException(e);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }

        /// <summary>
        /// Dictionaries only support translation between Chinese and English in translation results supported Britain, Japan and South Korea, France, Russia and the West to Chinese translation and Chinese to English translation
        /// about Q&A http://fanyi.youdao.com/openapi?path=faq
        /// about document http://fanyi.youdao.com/openapi?path=data-mode
        /// </summary>
        /// <param name="text">text's length must between 0 and 1000</param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        private YoudaoPostTransResult TranslateByPost(string text, string @from = "EN", string to = "ZH_CN")
        {
            if (!(text.Length > 0 && text.Length < 2000))
            {
                throw new Exception("text'length must between 0 and 2000");
            }
            try
            {
                //http://fanyi.youdao.com/translate?smartresult=dict&smartresult=rule&smartresult=ugc&sessionFrom=https://www.baidu.com/link

                string uri = "http://fanyi.youdao.com/translate";
                string type = @from + "2" + to;
                if (@from == Auto)
                {
                    type = @from;
                }
                string requestDetails = $"type={type}&i={HttpUtility.UrlEncode(Encoding.GetEncoding("ISO-8859-1").GetString(Encoding.UTF8.GetBytes(text)))}&doctype=json&xmlVersion=1.8&keyfrom=fanyi.web&ue=UTF-8&action=FY_BY_CLICKBUTTON&typoResult=true";
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(uri);
                httpWebRequest.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
                httpWebRequest.Method = "POST";
                byte[] bytes = Encoding.ASCII.GetBytes(requestDetails);
                httpWebRequest.ContentLength = bytes.Length;
                using (Stream outputStream = httpWebRequest.GetRequestStream())
                {
                    outputStream.Write(bytes, 0, bytes.Length);
                }
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
                        DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(YoudaoPostTransResult));
                        //Get deserialized object from JSON stream
                        YoudaoPostTransResult youdaoTransResult = (YoudaoPostTransResult)serializer.ReadObject(stream);
                        return youdaoTransResult;
                    }
                }
                catch
                {
                    throw;
                }
                finally
                {
                    response?.Close();
                }
            }
            catch (WebException e)
            {
                Utils.WebException.ProcessWebException(e);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }

        public string GetIdentity()
        {
            return "Youdao";
        }
        public static string GetName()
        {
            return "Youdao Translator / 有道翻译";
        }
        public static string GetChineseLanguage()
        {
            return Chinese;
        }


        public static string GetDescription()
        {
            return "1. text's length must between 0 and 200 \r\n2. 1000 free access per hour, over visits will be conducted in ban suspended, will resume after 1 hour.\r\n3. Free daily flow of 100,000 characters, more than RMB 50 parts per million characters; for example, a cumulative translation 600,000 characters, you will need to charge RMB 25, you can on the website translation http://fanyi.youdao.com/";
            //1. 每小时1000次免费访问，超过访问次数后会进行封禁暂停服务，1小时后会自然恢复。\r\n2. 每日免费10万字符流量，超过部分每百万字符50元人民币；例如，一天累积翻译60万字符，则需要收费25元人民币
        }
        public static string GetWebsite()
        {
            return "http://fanyi.youdao.com/";
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
            else if (!((@from != Chinese && to == Chinese) || (@from == Chinese && to == English)))
            {
                result.TranslationResultTypes = TranslationResultTypes.Failed;
                result.FailedReason = "Dictionaries only support translation between Chinese and English in translation results supported Britain, Japan and South Korea, France, Russia and the West to Chinese translation and Chinese to English translation";
            }
            else
            {
                try
                {
                    #region http api
                    //YoudaoTransResult youdaoTransResult = TranslateByHttp(text);
                    //if (youdaoTransResult == null || youdaoTransResult.ErrorCode != ErrorCodes.Normal)
                    //{
                    //    result.TranslationResultTypes = TranslationResultTypes.Failed;
                    //    result.FailedReason = "translate failed";
                    //}
                    //else
                    //{
                    //    result.TranslationResultTypes = TranslationResultTypes.Successed;
                    //    result.TargetText = youdaoTransResult.Translation[0];
                    //}
                    #endregion

                    #region post website

                    YoudaoPostTransResult youdaoTransResult = TranslateByPost(text, @from, to);
                    if (youdaoTransResult == null || youdaoTransResult.ErrorCode != ErrorCodes.Normal)
                    {
                        result.TranslationResultTypes = TranslationResultTypes.Failed;
                        result.FailedReason = "translate failed";
                    }
                    else
                    {
                        result.TranslationResultTypes = TranslationResultTypes.Successed;
                        result.TargetText = string.Empty;
                        foreach (var translateResult in youdaoTransResult.TranslateResults ?? new List<List<YoudaoTranslation>>())
                        {
                            foreach (var youdaoTranslation in translateResult)
                            {
                                result.TargetText += youdaoTranslation.Tgt;
                            }
                            result.TargetText += "\r\n";
                        }
                        if (result.TargetText.Length > 0)
                        {
                            result.TargetText = result.TargetText.Substring(0, result.TargetText.Length - 2);
                        }

                        var langs = youdaoTransResult.Type.Split('2');
                        result.SourceLanguage = langs[0];
                        result.TargetLanguage = langs[1];
                    }
                    #endregion
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