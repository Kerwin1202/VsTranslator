using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;

namespace VsTranslator.Settings
{
    public class Settings
    {
        public int ServiceIndex { get; set; }

        public TransSettings GoogleSettings { get; set; }

        public TransSettings BingSettings { get; set; }

        public TransSettings BaiduSettings { get; set; }

        public TransSettings YoudaoSettings { get; set; }

        public List<Spliter> LetterSpliters { get; set; }

        public string TranslateCachePath { get; set; }

        public readonly static string TranslateCacheDefaultPath = Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "VsTranslator"), "TranslateCache");

        public static Settings Instance()
        {
            return new Settings()
            {
                ServiceIndex = 1,
                BaiduSettings = new TransSettings()
                {
                    AppClient = new AppClient() { AppKey = "", ClientSecret = "" },
                    LastLanguageIndex = 2,
                    SourceLanguageIndex = 0,
                    TargetLanguageIndex = 0
                },
                BingSettings = new TransSettings()
                {
                    AppClient = new AppClient()
                    {
                        AppKey = "",
                        ClientSecret = ""
                    },
                    LastLanguageIndex = 12,
                    SourceLanguageIndex = 0,
                    TargetLanguageIndex = 5
                }
                    ,
                YoudaoSettings = new TransSettings()
                {
                    AppClient = new AppClient() { AppKey = "", ClientSecret = "" },
                    LastLanguageIndex = 1,
                    SourceLanguageIndex = 0,
                    TargetLanguageIndex = 0
                },
                GoogleSettings = new TransSettings()
                {
                    LastLanguageIndex = 12,
                    SourceLanguageIndex = 0,
                    TargetLanguageIndex = 6
                },
                LetterSpliters = new List<Spliter>()
                {
                    new Spliter()
                    {
                        Example = "MicrosoftTranslator",
                        MatchRegex = "([a-z])([A-Z])",
                        ReplaceRegex = "$1 $2"
                    },
                    new Spliter()
                    {
                        Example = "Microsoft_Translator",
                        MatchRegex = "_",
                        ReplaceRegex = " "
                    }
                },
                TranslateCachePath = TranslateCacheDefaultPath
            };
        }
    }

    public class AppClient
    {
        public string AppKey { get; set; }

        public string ClientSecret { get; set; }
    }

    public class TransSettings
    {
        public int SourceLanguageIndex { get; set; }

        public int TargetLanguageIndex { get; set; }

        public int LastLanguageIndex { get; set; }

        public AppClient AppClient { get; set; }
    }

    public class Spliter
    {
        public string MatchRegex { get; set; }

        public string ReplaceRegex { get; set; }

        public string Example { get; set; }
    }
}