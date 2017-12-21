using System;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading;
using System.Web;

namespace Translate.Core.Translator.Bing.Entities.Api
{
    internal class BingAdmAuth
    {
        private BingAdmAuth()
        {
            
        }

        internal static readonly string DatamarketAccessUri = "https://datamarket.accesscontrol.windows.net/v2/OAuth2-13";
        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly string _request;
        private AdmAccessToken _token;
        private readonly Timer _accessTokenRenewer;
        //Access token expires every 10 minutes. Renew it every 9 minutes only.
        private const int RefreshTokenDuration = 9;

        internal BingAdmAuth(string clientId, string clientSecret)
        {
            _clientId = clientId;
            _clientSecret = clientSecret;
            //If clientid or client secret has special characters, encode before sending request
            _request = $"grant_type=client_credentials&client_id={HttpUtility.UrlEncode(clientId)}&client_secret={HttpUtility.UrlEncode(_clientSecret)}&scope=http://api.microsofttranslator.com";
            _token = HttpPost(DatamarketAccessUri, _request);
            //renew the token every specified minutes
            _accessTokenRenewer = new Timer(new TimerCallback(OnTokenExpiredCallback), this, TimeSpan.FromMinutes(RefreshTokenDuration), TimeSpan.FromMilliseconds(-1));
        }

        internal AdmAccessToken GetAccessToken()
        {
            return _token;
        }

        private void RenewAccessToken()
        {
            AdmAccessToken newAccessToken = HttpPost(DatamarketAccessUri, _request);
            //swap the new token with old one
            //Note: the swap is thread unsafe
            _token = newAccessToken;
            Console.WriteLine($"Renewed token for user: {_clientId} is: {_token.AccessToken}");
        }

        private void OnTokenExpiredCallback(object stateInfo)
        {
            try
            {
                RenewAccessToken();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed renewing access token. Details: {ex.Message}");
            }
            finally
            {
                try
                {
                    _accessTokenRenewer.Change(TimeSpan.FromMinutes(RefreshTokenDuration), TimeSpan.FromMilliseconds(-1));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to reschedule the timer to renew access token. Details: {ex.Message}");
                }
            }
        }

        private AdmAccessToken HttpPost(string datamarketAccessUri, string requestDetails)
        {
            //Prepare OAuth request 
            WebRequest webRequest = WebRequest.Create(datamarketAccessUri);
            webRequest.ContentType = "application/x-www-form-urlencoded";
            webRequest.Method = "POST";
            byte[] bytes = Encoding.ASCII.GetBytes(requestDetails);
            webRequest.ContentLength = bytes.Length;
            webRequest.Timeout = 15*1000;
            using (Stream outputStream = webRequest.GetRequestStream())
            {
                outputStream.Write(bytes, 0, bytes.Length);
            }
            using (WebResponse webResponse = webRequest.GetResponse())
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(AdmAccessToken));
                //Get deserialized object from JSON stream
                AdmAccessToken token = (AdmAccessToken)serializer.ReadObject(webResponse.GetResponseStream());
                return token;
            }
        }
    }
}