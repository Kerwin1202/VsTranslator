using System;
using System.IO;
using System.Text;
using System.Web;
using Translate.Core.Translator.Google;
using Translate.Core.Translator.Utils;

namespace Translate.Core.Translator
{
    using static GoogleUtils;

    public static class TextToSpeech
    {
        /// <summary>
        /// Text to Sppech
        /// </summary>
        /// <param name="text"></param>
        /// <param name="lang"></param>
        /// <returns></returns>
        public static byte[] Text2Speech(string text, string lang)
        {
            //https://translate.google.cn/translate_tts?ie=UTF-8&q=hello%20world&tl=en&total=1&idx=0&textlen=11&tk=288080.147058&client=t&prev=input

            text = text.Replace("\\", "");
            var tk = GetTk(text);
            var result = new HttpHelper().GetHtml(new HttpItem()
            {
                Url =
                     $"https://translate.google.cn/translate_tts?ie=UTF-8&q={HttpUtility.UrlEncode(text)}&tl={lang}&total=1&idx=0&textlen={text.Length}&tk={tk}&client=t&prev=input",
                Referer = "https://translate.google.cn/",
                UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/59.0.3071.115 Safari/537.36",
                ResultType = ResultType.Byte
            });
            return result.ResultByte;
        }

        /// <summary>
        /// Text to Speech
        /// </summary>
        /// <param name="text"></param>
        /// <param name="lang"></param>
        /// <param name="savePath">the path of save</param>
        /// <returns></returns>
        public static void Text2Speech(string text, string lang, string savePath)
        {
            var buffer = Text2Speech(text, lang);
            if (buffer == null)
            {
                return;
            }
            File.WriteAllBytes(savePath, buffer);
        }
    }
}