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


        public static ITranslator GetTranslator(int commandId)
        {
            ITranslator translator = _googleTranslator;
            switch (commandId)
            {
                case (int)PkgCmdIdList.GoogleTranslate:
                    if (_googleTranslator == null)
                    {
                        _googleTranslator = new GoogleTranslator();
                    }
                    translator = _googleTranslator;
                    break;
                case (int)PkgCmdIdList.BingTranslate:
                    if (_bingTranslator == null)
                    {
                        _bingTranslator = new BingTranslator("VsTranslatorByKerwin", "LOAgOYu99LyNzVoa+LL53zIk93RFhAaZQxwtSW+an5E=");
                    }
                    translator = _bingTranslator;
                    break;
                case (int)PkgCmdIdList.BaiduTranslate:
                    if (_baiduTranslator == null)
                    {
                        _baiduTranslator = new BaiduTranslator("20161214000033991", "HMlukU9THx2Twx1I14Hz");
                    }
                    translator = _baiduTranslator;
                    break;
                case (int)PkgCmdIdList.YoudaoTranslate:
                    if (_youdaoTranslator == null)
                    {
                        _youdaoTranslator = new YoudaoTranslator("zhiyue", "702916626");
                    }
                    translator = _youdaoTranslator;
                    break;
            }
            return translator;
        }

    }
}