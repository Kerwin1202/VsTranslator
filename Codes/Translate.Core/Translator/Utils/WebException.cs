using System;
using System.IO;
using System.Net;
using System.Text;

namespace Translate.Core.Translator.Utils
{
    internal class WebException
    {
        internal static void ProcessWebException(System.Net.WebException e)
        {
            Console.WriteLine("{0}", e.ToString());
            // Obtain detailed error information
            string strResponse;
            using (HttpWebResponse response = (HttpWebResponse)e.Response)
            {
                using (Stream responseStream = response.GetResponseStream())
                {
                    if (responseStream == null)
                    {
                        return;
                    }
                    using (StreamReader sr = new StreamReader(responseStream, Encoding.ASCII))
                    {
                        strResponse = sr.ReadToEnd();
                    }
                }
            }
            Console.WriteLine("Http status code={0}, error message={1}", e.Status, strResponse);
        }
    }
}