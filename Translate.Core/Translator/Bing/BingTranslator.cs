using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web;
using Translate.Core.Translator.Bing.Entities;
using Translate.Core.Translator.Entities;
using Translate.Core.Translator.Enums;

namespace Translate.Core.Translator.Bing
{
    public class BingTranslator : ITranslator
    {
        private static AdmAccessToken _admToken;
        private static BingAdmAuth _admAuth;

        private static readonly List<TranslationLanguage> TargetLanguages;
        private static readonly List<TranslationLanguage> SourceLanguages;

        private BingTranslator()
        {

        }

        static BingTranslator()
        {
            //language list from https://msdn.microsoft.com/en-us/library/hh456380.aspx

            TargetLanguages = new List<TranslationLanguage>()
            {
                new TranslationLanguage("af","Afrikaans / 南非荷兰语"),
                new TranslationLanguage("ar", "Arabic / 阿拉伯语"),
                new TranslationLanguage("bs-Latn", "Bosnian (Latin) / 波斯尼亚 (拉丁语)"),
                new TranslationLanguage("bg", "Bulgarian / 保加利亚语"),
                new TranslationLanguage("ca", "Catalan / 加泰罗尼亚语"),
                new TranslationLanguage("zh-CHS", "Chinese (Simplified) / 简体中文"),
                new TranslationLanguage("zh-CHT", "Chinese (Traditional) / 繁体中文"),
                new TranslationLanguage("yue", "Cantonese (Traditional) / 粤语（繁体）"),
                new TranslationLanguage("hr", "Croatian / 克罗地亚语"),
                new TranslationLanguage("cs", "Czech / 捷克语"),
                new TranslationLanguage("da", "Danish / 丹麦语"),
                new TranslationLanguage("nl", "Dutch / 荷兰语"),
                new TranslationLanguage("en", "English / 英语"),
                new TranslationLanguage("et", "Estonian / 爱沙尼亚语"),
                new TranslationLanguage("fj", "Fijian / 斐济语"),
                new TranslationLanguage("fil", "Filipino / 菲律宾语"),
                new TranslationLanguage("fi", "Finnish / 芬兰语"),
                new TranslationLanguage("fr", "French / 法语"),
                new TranslationLanguage("de", "German / 德语"),
                new TranslationLanguage("el", "Greek / 希腊语"),
                new TranslationLanguage("ht", "Haitian Creole / 海地克里奥尔语"),
                new TranslationLanguage("he", "Hebrew / 希伯来语"),
                new TranslationLanguage("hi", "Hindi / 印地语"),
                new TranslationLanguage("mww", "Hmong Daw /苗语"),
                new TranslationLanguage("hu", "Hungarian / 匈牙利语"),
                new TranslationLanguage("id", "Indonesian / 印度尼西亚语"),
                new TranslationLanguage("it", "Italian / 意大利语"),
                new TranslationLanguage("ja", "Japanese / 日语"),
                new TranslationLanguage("sw", "Kiswahili / 斯瓦希里语"),
                new TranslationLanguage("tlh", "Klingon / 克林贡语"),
                new TranslationLanguage("ko", "Korean / 韩语"),
                new TranslationLanguage("lv", "Latvian / 拉脱维亚语"),
                new TranslationLanguage("lt", "Lithuanian / 立陶宛语"),
                new TranslationLanguage("mg", "Malagasy / 马尔加什语"),
                new TranslationLanguage("ms", "Malay / 马来语"),
                new TranslationLanguage("mt", "Maltese / 马耳他语"),
                new TranslationLanguage("yua", "Yucatec Maya / 玛雅语"),
                new TranslationLanguage("no", "Norwegian Bokmål / 挪威博克马尔语"),
                new TranslationLanguage("fa", "Persian / 波斯语"),
                new TranslationLanguage("pl", "Polish / 波兰语"),
                new TranslationLanguage("pt", "Portuguese / 葡萄牙语"),
                new TranslationLanguage("ro", "Romanian / 罗马尼亚语"),
                new TranslationLanguage("ru", "Russian / 俄语"),
                new TranslationLanguage("sm", "Samoan / 萨摩亚语"),
                new TranslationLanguage("sr-Cyrl", "Serbian (Cyrillic) / 塞尔维亚语"),
                new TranslationLanguage("sr-Latn", "Serbian (Latin) / 塞尔维亚语（拉丁语）"),
                new TranslationLanguage("sk", "Slovak / 斯洛伐克语"),
                new TranslationLanguage("sl", "Slovenian / 斯洛文尼亚语"),
                new TranslationLanguage("es", "Spanish / 西班牙语"),
                new TranslationLanguage("sv", "Swedish / 瑞典语"),
                new TranslationLanguage("ty", "Tahitian / 大溪地语 (塔希提岛)"),
                new TranslationLanguage("th", "Thai / 泰语"),
                new TranslationLanguage("tr", "Turkish / 土耳其语"),
                new TranslationLanguage("uk", "Ukrainian / 乌克兰语"),
                new TranslationLanguage("ur", "Urdu / 乌尔都语"),
                new TranslationLanguage("vi", "Vietnamese / 越南语"),
                new TranslationLanguage("cy", "Welsh / 威尔士语")
            };
            SourceLanguages = new List<TranslationLanguage>() { new TranslationLanguage("", "Auto-detect / 自动检测") };
            SourceLanguages.AddRange(TargetLanguages);
        }

        //soap : https://msdn.microsoft.com/en-us/library/ff512435.aspx
        //ajax : https://msdn.microsoft.com/en-us/library/ff512404.aspx
        //http : https://msdn.microsoft.com/en-us/library/ff512419.aspx

        public BingTranslator(string clientId, string clientSecret)
        {
            //Get Client Id and Client Secret from https://datamarket.azure.com/developer/applications/
            //Refer obtaining AccessToken (http://msdn.microsoft.com/en-us/library/hh454950.aspx) 

            if (string.IsNullOrWhiteSpace(clientId) || string.IsNullOrWhiteSpace(clientSecret))
            {
                throw new Exception("app id and client secret is necessary");
            }
            _admAuth = new BingAdmAuth(clientId, clientSecret);


        }

        /// <summary>
        /// https://msdn.microsoft.com/en-us/library/ff512419.aspx
        /// </summary>
        /// <param name="text"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        private string TranslateByHttp(string text, string from = "en", string to = "zh-CHS")
        {
            try
            {
                //var sw = Stopwatch.StartNew();
                var authToken = GetAuthToken();
                string uri = "http://api.microsofttranslator.com/v2/Http.svc/Translate?text=" + HttpUtility.UrlEncode(text) + "&from=" + from + "&to=" + to;
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(uri);
                httpWebRequest.Headers.Add("Authorization", authToken);
                WebResponse response = null;
                try
                {
                    response = httpWebRequest.GetResponse();
                    using (Stream stream = response.GetResponseStream())
                    {
                        if (stream == null)
                        {
                            return string.Empty;
                        }
                        DataContractSerializer dcs = new DataContractSerializer(typeof(String));
                        string translation = (string)dcs.ReadObject(stream);

                        Console.WriteLine("Bing translation for source text '{0}' from {1} to {2} is", text, from, to);
                        Console.WriteLine(translation);

                        return translation;
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
            return String.Empty;
        }
        /// <summary>
        /// https://msdn.microsoft.com/en-us/library/ff512404.aspx
        /// </summary>
        /// <param name="text"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        private BingTransResult TranslateByAjax(string text, string from = "en", string to = "zh-CHS")
        {
            try
            {
                var authToken = GetAuthToken();
                string uri = "http://api.microsofttranslator.com/v2/ajax.svc/GetTranslations?text=" + HttpUtility.UrlEncode(text) + "&from=" + from + "&to=" + to + "&maxTranslations=20";

                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(uri);
                httpWebRequest.Headers.Add("Authorization", authToken);
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
                        StreamReader streamReader = new StreamReader(stream, Encoding.UTF8);
                        string jsonString = streamReader.ReadToEnd();
                        using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonString)))
                        {
                            BingTransResult translation = (BingTransResult)new DataContractJsonSerializer(typeof(BingTransResult)).ReadObject(ms);
                            Console.WriteLine("Bing translation for source text '{0}' from {1} to {2} is", text, from, to);
                            Console.WriteLine(translation);
                            return translation;
                        }
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

        private string GetAuthToken()
        {
            _admToken = _admAuth.GetAccessToken();
            // Create a header with the access_token property of the returned token
            return "Bearer " + _admToken.AccessToken;
        }


        //http://api.microsofttranslator.com/v2/ajax.svc/

        //get Support languages : http://api.microsofttranslator.com/v2/ajax.svc/GetLanguagesForTranslate
        //get support languages's names : http://api.microsofttranslator.com/v2/ajax.svc/GetLanguageNames?locale=en&languageCodes=["af","ar","bs-Latn","bg","ca","zh-CHS","zh-CHT","yue","hr","cs","da","nl","en","et","fj","fil","fi","fr","de","el","ht","he","hi","mww","hu","id","it","ja","sw","tlh","tlh-Qaak","ko","lv","lt","mg","ms","mt","yua","no","otq","fa","pl","pt","ro","ru","sm","sr-Cyrl","sr-Latn","sk","sl","es","sv","ty","th","to","tr","uk","ur","vi","cy"]

        public  string GetIdentity()
        {
            return "Bing";
        }
        public static string GetName()
        {
            return "Bing Translator / 必应翻译";
        }
        public static string GetChineseLanguage()
        {
            return "zh-CHS";
        }


        public static string GetDescription()
        {
            return "you can on the website translation http://www.bing.com/translator/";
        }

        public static string GetWebsite()
        {
            return "http://www.bing.com/translator/";
        }

        public static List<TranslationLanguage> GetTargetLanguages()
        {
            return TargetLanguages;
        }

        public static List<TranslationLanguage> GetSourceLanguages()
        {
            return SourceLanguages;
        }

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
            else
            {
                try
                {
                    result.TranslationResultTypes = TranslationResultTypes.Successed;

                    #region ajax
                    BingTransResult bingTransResult = TranslateByAjax(text, from, to);
                    result.SourceLanguage = bingTransResult?.From;
                    result.TargetText = bingTransResult?.Translations?[0].TranslatedText;
                    #endregion

                    #region http
                    //result.TargetText = TranslateByHttp(text, from, to); 
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