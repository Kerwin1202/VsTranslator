using System.Text.RegularExpressions;
using VsTranslator.Core.Translator.Baidu;
using VsTranslator.Core.Translator.Bing;
using VsTranslator.Core.Translator.Google;
using VsTranslator.Core.Translator.Youdao;
using VsTranslator.Settings;

namespace VsTranslator.Core.Translator
{
    public class TranslatorFactory
    {
        private static ITranslator _googleTranslator;

        private static ITranslator _bingTranslator;

        private static ITranslator _baiduTranslator;

        private static ITranslator _youdaoTranslator;

        /// <summary>
        /// 不含标点
        /// </summary>
        private static readonly Regex ChineseRegex = new Regex(@"[\u4e00-\u9fa5]");

        public static string GetSourceLanguage(int commandId, string selectedText)
        {
            if (ChineseRegex.IsMatch(selectedText))
            {
                return GetChineseLanguage(commandId);
            }
            string sourceLanguage = string.Empty;

            switch (commandId)
            {
                case (int)PkgCmdIdList.GoogleTranslate:
                    sourceLanguage =
                        GoogleTranslator.GetSourceLanguages()[OptionsSettings.Settings.GoogleSettings.SourceLanguageIndex].Code;
                    break;
                case (int)PkgCmdIdList.BingTranslate:
                    sourceLanguage =
                        BingTranslator.GetSourceLanguages()[OptionsSettings.Settings.BingSettings.SourceLanguageIndex].Code;
                    break;
                case (int)PkgCmdIdList.BaiduTranslate:
                    sourceLanguage =
                        BaiduTranslator.GetSourceLanguages()[OptionsSettings.Settings.BaiduSettings.SourceLanguageIndex].Code;
                    break;
                case (int)PkgCmdIdList.YoudaoTranslate:
                    sourceLanguage =
                        YoudaoTranslator.GetSourceLanguages()[OptionsSettings.Settings.YoudaoSettings.SourceLanguageIndex].Code;
                    break;
            }
            return sourceLanguage;
        }

        public static string GetTargetLanguage(int commandId, string selectedText)
        {
            string targetLanguage = string.Empty;
            if (ChineseRegex.IsMatch(selectedText))
            {
                switch (commandId)
                {
                    case (int)PkgCmdIdList.GoogleTranslate:
                        targetLanguage =
                            GoogleTranslator.GetTargetLanguages()[OptionsSettings.Settings.GoogleSettings.LastLanguageIndex].Code;
                        break;
                    case (int)PkgCmdIdList.BingTranslate:
                        targetLanguage =
                            BingTranslator.GetTargetLanguages()[OptionsSettings.Settings.BingSettings.LastLanguageIndex].Code;
                        break;
                    case (int)PkgCmdIdList.BaiduTranslate:
                        targetLanguage =
                            BaiduTranslator.GetTargetLanguages()[OptionsSettings.Settings.BaiduSettings.LastLanguageIndex].Code;
                        break;
                    case (int)PkgCmdIdList.YoudaoTranslate:
                        targetLanguage =
                            YoudaoTranslator.GetTargetLanguages()[OptionsSettings.Settings.YoudaoSettings.LastLanguageIndex].Code;
                        break;
                }
            }
            else
            {
                switch (commandId)
                {
                    case (int)PkgCmdIdList.GoogleTranslate:
                        targetLanguage =
                            GoogleTranslator.GetTargetLanguages()[OptionsSettings.Settings.GoogleSettings.TargetLanguageIndex].Code;
                        break;
                    case (int)PkgCmdIdList.BingTranslate:
                        targetLanguage =
                            BingTranslator.GetTargetLanguages()[OptionsSettings.Settings.BingSettings.TargetLanguageIndex].Code;
                        break;
                    case (int)PkgCmdIdList.BaiduTranslate:
                        targetLanguage =
                            BaiduTranslator.GetTargetLanguages()[OptionsSettings.Settings.BaiduSettings.TargetLanguageIndex].Code;
                        break;
                    case (int)PkgCmdIdList.YoudaoTranslate:
                        targetLanguage =
                            YoudaoTranslator.GetTargetLanguages()[OptionsSettings.Settings.YoudaoSettings.TargetLanguageIndex].Code;
                        break;
                }
            }
            return targetLanguage;
        }


        private static string GetChineseLanguage(int commandId)
        {
            string chineseLanguage = string.Empty;
            switch (commandId)
            {
                case (int)PkgCmdIdList.GoogleTranslate:
                    chineseLanguage = GoogleTranslator.GetChineseLanguage();
                    break;
                case (int)PkgCmdIdList.BingTranslate:
                    chineseLanguage = BingTranslator.GetChineseLanguage();
                    break;
                case (int)PkgCmdIdList.BaiduTranslate:
                    chineseLanguage = BaiduTranslator.GetChineseLanguage();
                    break;
                case (int)PkgCmdIdList.YoudaoTranslate:
                    chineseLanguage = YoudaoTranslator.GetChineseLanguage();
                    break;
            }
            return chineseLanguage;
        }


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
                        _bingTranslator = new BingTranslator(OptionsSettings.Settings.BingSettings.AppClient.AppKey, OptionsSettings.Settings.BingSettings.AppClient.ClientSecret);
                    }
                    translator = _bingTranslator;
                    break;
                case (int)PkgCmdIdList.BaiduTranslate:
                    if (_baiduTranslator == null)
                    {
                        _baiduTranslator = new BaiduTranslator(OptionsSettings.Settings.BaiduSettings.AppClient.AppKey, OptionsSettings.Settings.BaiduSettings.AppClient.ClientSecret);
                    }
                    translator = _baiduTranslator;
                    break;
                case (int)PkgCmdIdList.YoudaoTranslate:
                    if (_youdaoTranslator == null)
                    {
                        _youdaoTranslator = new YoudaoTranslator();
                        //OptionsSettings.Settings.YoudaoSettings.AppClient.AppKey, OptionsSettings.Settings.YoudaoSettings.AppClient.ClientSecret
                    }
                    translator = _youdaoTranslator;
                    break;
            }
            return translator;
        }

    }
}