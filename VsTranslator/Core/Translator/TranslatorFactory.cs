using VsTranslator.Core.Translator.Baidu;
using VsTranslator.Core.Translator.Bing;
using VsTranslator.Core.Translator.Entities;
using VsTranslator.Core.Translator.Google;
using VsTranslator.Core.Translator.Youdao;

namespace VsTranslator.Core.Translator
{
    public class TranslatorFactory
    {
        private static ITranslator _googleTranslator;

        private static ITranslator _bingTranslator;

        private static ITranslator _baiduTranslator;

        private static ITranslator _youdaoTranslator;

        //这里不直接把实例化代码放在上面直接实例化是因为,防止首次翻译的时候因为其中某个翻译实例化异常导致整个翻译不了
        public static TranslationResult TranslateByGoogle(string text, string @from = "en", string to = "zh-CN")
        {
            if (_googleTranslator == null)
            {
                _googleTranslator = new GoogleTranslator();
            }
            return _googleTranslator.Translate(text, @from, to);
        }

        public static TranslationResult TranslateByBing(string text, string @from = "en", string to = "zh-CHS")
        {
            if (_bingTranslator == null)
            {
                _bingTranslator = new BingTranslator("VsTranslatorByKerwin", "LOAgOYu99LyNzVoa+LL53zIk93RFhAaZQxwtSW+an5E=");
            }
            return _bingTranslator.Translate(text, @from, to);
        }

        public static TranslationResult TranslateByBaidu(string text, string @from = "en", string to = "zh")
        {
            if (_baiduTranslator == null)
            {
                _baiduTranslator = new BaiduTranslator("20161214000033991", "HMlukU9THx2Twx1I14Hz");
            }
            return _baiduTranslator.Translate(text, @from, to);
        }

        public static TranslationResult TranslateByYoudao(string text, string @from = "EN", string to = "ZH_CN")
        {
            if (_youdaoTranslator==null)
            {
                _youdaoTranslator = new YoudaoTranslator("zhiyue", "702916626");
            }
            return _youdaoTranslator.Translate(text, @from, to);
        }

    }
}