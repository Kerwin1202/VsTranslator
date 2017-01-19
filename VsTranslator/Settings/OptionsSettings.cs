using System;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace VsTranslator.Settings
{
    public class OptionsSettings
    {
        private static Settings _settings;

        private static Settings Settings
        {
            get
            {
                return _settings ?? (_settings = new Settings()
                {
                    ServiceIndex = 1,
                    BaiduSettings = new TransSettings()
                    {
                        AppClient =
                            new AppClient() { AppKey = "20161214000033991", ClientSecret = "HMlukU9THx2Twx1I14Hz" },
                        LastLanguageIndex = 2,
                        SourceLanguageIndex = 0,
                        TargetLanguageIndex = 0
                    },
                    BingSettings = new TransSettings()
                    {
                        AppClient =
                            new AppClient()
                            {
                                AppKey = "VsTranslatorByKerwin",
                                ClientSecret = "LOAgOYu99LyNzVoa+LL53zIk93RFhAaZQxwtSW+an5E="
                            },
                        LastLanguageIndex = 12,
                        SourceLanguageIndex = 0,
                        TargetLanguageIndex = 5
                    }
                    ,
                    YoudaoSettings = new TransSettings()
                    {
                        AppClient = new AppClient() { AppKey = "zhiyue", ClientSecret = "702916626" },
                        LastLanguageIndex = 1,
                        SourceLanguageIndex = 0,
                        TargetLanguageIndex = 0
                    },
                    GoogleSettings = new TransSettings()
                    {
                        LastLanguageIndex = 12,
                        SourceLanguageIndex = 0,
                        TargetLanguageIndex = 6
                    }
                });
            }

            set
            {
                _settings = value;
                File.WriteAllText(ConfigPath, JsonConvert.SerializeObject(_settings),Encoding.UTF8);
            }
        }

        private static readonly string LocalPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "VsTranslator");

        private static readonly string ConfigPath = Path.Combine(LocalPath, "settings.config");

        static OptionsSettings()
        {
            if (!Directory.Exists(LocalPath))
            {
                Directory.CreateDirectory(LocalPath);
            }

            if (File.Exists(ConfigPath))
            {
                var settings = File.ReadAllText(ConfigPath, Encoding.UTF8);
                if (!string.IsNullOrWhiteSpace(settings))
                {
                    try
                    {
                        _settings = JsonConvert.DeserializeObject<Settings>(settings);
                    }
                    catch (Exception)
                    {
                        _settings = null;
                    }
                }
            }
        }

        public static void ShowOptions()
        {
            new TranslateOptions(Settings)
            {
                OnSave = SaveSettings
            }.ShowDialog();
        }

        public static void SaveSettings(Settings settings)
        {
            Settings = settings;
        }
    }
}