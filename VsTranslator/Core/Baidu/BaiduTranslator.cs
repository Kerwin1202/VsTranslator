using System;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using VsTranslator.Core.Baidu.Entities;
using VsTranslator.Core.Bing.Entities;
using VsTranslator.Core.Utils;
using WebException = System.Net.WebException;

namespace VsTranslator.Core.Baidu
{
    public class BaiduTranslator
    {
        private BaiduTranslator()
        {
            
        }

        private readonly string TranslateUrl = "http://api.fanyi.baidu.com/api/trans/vip/translate";
        //Get appid and client secret from http://api.fanyi.baidu.com/api/trans/product/index
        private readonly string _appid;
        private readonly string _clientSecret;


        public BaiduTranslator(string appid,string clientSecret)
        {
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
        public BaiduTransResult Translate(string text,string from = "en", string to= "zh")
        {
            if (!(text.Length>0&& text.Length<2000))
            {
                return null;
            }
            try
            {
                var timestamp = Times.TimeStampWithMsec;
                string uri = $"{TranslateUrl}?q={ System.Web.HttpUtility.UrlEncode(text)}&from={from}&to={to}&appid={_appid}&salt={timestamp}&sign={Encrypts.CreateMd5EncryptFromString($"{_appid}{text}{timestamp}{_clientSecret}",Encoding.UTF8)}";
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
                            //
                        }
                        return baiduTransResult;
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