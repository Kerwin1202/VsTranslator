namespace VsTranslator.Settings
{
    public class Settings
    {
        public int ServiceIndex { get; set; }

        public TransSettings GoogleSettings { get; set; }

        public TransSettings BingSettings { get; set; }

        public TransSettings BaiduSettings { get; set; }

        public TransSettings YoudaoSettings { get; set; }


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
}