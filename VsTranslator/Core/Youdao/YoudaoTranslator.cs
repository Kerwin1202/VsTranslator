using System;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web;
using VsTranslator.Core.Baidu.Entities;
using VsTranslator.Core.Utils;
using VsTranslator.Core.Youdao.Entities;
using VsTranslator.Core.Youdao.Enums;
using WebException = VsTranslator.Core.Utils.WebException;

namespace VsTranslator.Core.Youdao
{
    public class YoudaoTranslator
    {
        private YoudaoTranslator()
        {

        }

        private const string TranslateUrl = "http://fanyi.youdao.com/openapi.do";

        //Get appid and client secret from http://fanyi.youdao.com/openapi
        private readonly string _appid;

        private readonly string _clientSecret;

        public YoudaoTranslator(string appid, string clientSecret)
        {
            _appid = appid;
            _clientSecret = clientSecret;
        }

        /// <summary>
        /// Dictionaries only support translation between Chinese and English in translation results supported Britain, Japan and South Korea, France, Russia and the West to Chinese translation and Chinese to English translation
        /// about Q&A http://fanyi.youdao.com/openapi?path=faq
        /// about document http://fanyi.youdao.com/openapi?path=data-mode
        /// </summary>
        /// <param name="text">text's length must between 0 and 200</param>
        /// <param name="translateType"></param>
        public YoudaoTransResult Translate(string text, TranslateType translateType = TranslateType.Dict | TranslateType.Translate)
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
    }
}