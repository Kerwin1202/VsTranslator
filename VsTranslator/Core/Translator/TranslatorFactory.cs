using VsTranslator.Core.Translator.Baidu;
using VsTranslator.Core.Translator.Bing;
using VsTranslator.Core.Translator.Entities;
using VsTranslator.Core.Translator.Google;
using VsTranslator.Core.Translator.Youdao;

namespace VsTranslator.Core.Translator
{
    public class TranslatorFactory
    {
        private static readonly ITranslator GoogleTranslator = new GoogleTranslator();

        private static readonly ITranslator BingTranslator = new BingTranslator("VsTranslatorByKerwin", "LOAgOYu99LyNzVoa+LL53zIk93RFhAaZQxwtSW+an5E=");

        private static readonly ITranslator BaiduTranslator = new BaiduTranslator("20161214000033991", "HMlukU9THx2Twx1I14Hz");

        private static readonly ITranslator YoudaoTranslator = new YoudaoTranslator("zhiyue", "702916626");

        public static TranslationResult TranslateByGoogle(string text, string @from = "en", string to = "zh-CN")
        {
            return GoogleTranslator.Translate(text, @from, to);
        }

        public static TranslationResult TranslateByBing(string text, string @from = "en", string to = "zh-CHS")
        {
           return BingTranslator.Translate(text, @from, to);
        }

        public static TranslationResult TranslateByBaidu(string text, string @from = "en", string to = "zh")
        {
            return BaiduTranslator.Translate(text, @from, to);
        }

        public static TranslationResult TranslateByYoudao(string text, string @from = "EN", string to = "ZH_CN")
        {
            return YoudaoTranslator.Translate(text, @from, to);
        }

    }
}