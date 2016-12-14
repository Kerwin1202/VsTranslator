using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using VsTranslator.Core.Bing.Entities;

namespace VsTranslator.Core.Bing
{
    public class BingTranslator 
    {
        private static AdmAccessToken _admToken;
        private static BingAdmAuth _admAuth;

        private BingTranslator()
        {
            
        }

        public BingTranslator(string clientId, string clientSecret)
        {
            //Get Client Id and Client Secret from https://datamarket.azure.com/developer/applications/
            //Refer obtaining AccessToken (http://msdn.microsoft.com/en-us/library/hh454950.aspx) 
            _admAuth = new BingAdmAuth(clientId, clientSecret);
        }

        public string Translate(string text, string from = "en", string to = "zh-CHS")
        {
            try
            {
                var sw = Stopwatch.StartNew();
                var authToken = GetAuthToken();
                string uri = "http://api.microsofttranslator.com/v2/Http.svc/Translate?text=" + System.Web.HttpUtility.UrlEncode(text) + "&from=" + from + "&to=" + to;
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(uri);
                httpWebRequest.Headers.Add("Authorization", authToken);
                WebResponse response = null;
                try
                {
                    response = httpWebRequest.GetResponse();
                    using (Stream stream = response.GetResponseStream())
                    {
                        if (stream==null)
                        {
                            return string.Empty;
                        }
                        System.Runtime.Serialization.DataContractSerializer dcs = new System.Runtime.Serialization.DataContractSerializer(Type.GetType("System.String"));
                        string translation = (string)dcs.ReadObject(stream);
                        Console.WriteLine("Bing translation for source text '{0}' from {1} to {2} is", text, from, to);
                        Console.WriteLine(translation);
                        sw.Stop();
                        Console.WriteLine(sw.ElapsedMilliseconds);
                        return translation;
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
            return string.Empty;
        }

        private string GetAuthToken()
        {
            _admToken = _admAuth.GetAccessToken();
            // Create a header with the access_token property of the returned token
            return "Bearer " + _admToken.AccessToken;
        }


    }
}