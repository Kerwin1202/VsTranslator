using System;
using System.Text.RegularExpressions;
using Translate.Core.Translator;
using Translate.Core.Translator.Baidu;
using Translate.Core.Translator.Bing;
using Translate.Core.Translator.Google;
using Translate.Core.Translator.Youdao;

namespace Translate.Settings
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

        public static string GetSourceLanguage(TranslateType type, string selectedText)
        {
            if (ChineseRegex.IsMatch(selectedText))
            {
                return GetChineseLanguage(type);
            }
            string sourceLanguage = string.Empty;

            switch (type)
            {
                case TranslateType.Google:
                    sourceLanguage = GoogleTranslator.GetSourceLanguages()[OptionsSettings.Settings.GoogleSettings.SourceLanguageIndex].Code;
                    break;
                case TranslateType.Bing:
                    sourceLanguage = BingTranslator.GetSourceLanguages()[OptionsSettings.Settings.BingSettings.SourceLanguageIndex].Code;
                    break;
                case TranslateType.Baidu:
                    sourceLanguage = BaiduTranslator.GetSourceLanguages()[OptionsSettings.Settings.BaiduSettings.SourceLanguageIndex].Code;
                    break;
                case TranslateType.Youdao:
                    sourceLanguage = YoudaoTranslator.GetSourceLanguages()[OptionsSettings.Settings.YoudaoSettings.SourceLanguageIndex].Code;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
            return sourceLanguage;
        }

        public static string GetTargetLanguage(TranslateType type, string selectedText)
        {
            string targetLanguage = string.Empty;
            if (ChineseRegex.IsMatch(selectedText))
            {
                switch (type)
                {
                    case TranslateType.Google:
                        targetLanguage = GoogleTranslator.GetTargetLanguages()[OptionsSettings.Settings.GoogleSettings.LastLanguageIndex].Code;
                        break;
                    case TranslateType.Bing:
                        targetLanguage = BingTranslator.GetTargetLanguages()[OptionsSettings.Settings.BingSettings.LastLanguageIndex].Code;
                        break;
                    case TranslateType.Baidu:
                        targetLanguage = BaiduTranslator.GetTargetLanguages()[OptionsSettings.Settings.BaiduSettings.LastLanguageIndex].Code;
                        break;
                    case TranslateType.Youdao:
                        targetLanguage = YoudaoTranslator.GetTargetLanguages()[OptionsSettings.Settings.YoudaoSettings.LastLanguageIndex].Code;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(type), type, null);
                }
            }
            else
            {
                switch (type)
                {
                    case TranslateType.Google:
                        targetLanguage = GoogleTranslator.GetTargetLanguages()[OptionsSettings.Settings.GoogleSettings.TargetLanguageIndex].Code;
                        break;
                    case TranslateType.Bing:
                        targetLanguage = BingTranslator.GetTargetLanguages()[OptionsSettings.Settings.BingSettings.TargetLanguageIndex].Code;
                        break;
                    case TranslateType.Baidu:
                        targetLanguage = BaiduTranslator.GetTargetLanguages()[OptionsSettings.Settings.BaiduSettings.TargetLanguageIndex].Code;
                        break;
                    case TranslateType.Youdao:
                        targetLanguage = YoudaoTranslator.GetTargetLanguages()[OptionsSettings.Settings.YoudaoSettings.TargetLanguageIndex].Code;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(type), type, null);
                }
            }
            return targetLanguage;
        }


        private static string GetChineseLanguage(TranslateType type)
        {
            string chineseLanguage = string.Empty;
            switch (type)
            {
                case TranslateType.Google:
                    chineseLanguage = GoogleTranslator.GetChineseLanguage();
                    break;
                case TranslateType.Bing:
                    chineseLanguage = BingTranslator.GetChineseLanguage();
                    break;
                case TranslateType.Baidu:
                    chineseLanguage = BaiduTranslator.GetChineseLanguage();
                    break;
                case TranslateType.Youdao:
                    chineseLanguage = YoudaoTranslator.GetChineseLanguage();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
            return chineseLanguage;
        }


        public static ITranslator GetTranslator(TranslateType type)
        {
            ITranslator translator = _googleTranslator;
            switch (type)
            {
                case TranslateType.Google:
                    if (_googleTranslator == null)
                    {
                        _googleTranslator = new GoogleTranslator();
                    }
                    translator = _googleTranslator;
                    break;
                case TranslateType.Bing:
                    if (_bingTranslator == null)
                    {
                        _bingTranslator = new BingTranslator(OptionsSettings.Settings.BingSettings.AppClient.AppKey, OptionsSettings.Settings.BingSettings.AppClient.ClientSecret);
                    }
                    translator = _bingTranslator;
                    break;
                case TranslateType.Baidu:
                    if (_baiduTranslator == null)
                    {
                        _baiduTranslator = new BaiduTranslator(OptionsSettings.Settings.BaiduSettings.AppClient.AppKey, OptionsSettings.Settings.BaiduSettings.AppClient.ClientSecret);
                    }
                    translator = _baiduTranslator;
                    break;
                case TranslateType.Youdao:
                    if (_youdaoTranslator == null)
                    {
                        _youdaoTranslator = new YoudaoTranslator();
                        //OptionsSettings.Settings.YoudaoSettings.AppClient.AppKey, OptionsSettings.Settings.YoudaoSettings.AppClient.ClientSecret
                    }
                    translator = _youdaoTranslator;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
            return translator;
        }

    }
}