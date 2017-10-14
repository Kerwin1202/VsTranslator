using System;
using System.IO;
using System.Linq;
using System.Text;
using Translate.Core.Translator;
using Translate.Core.Translator.Utils;

namespace Translate.Settings.TTS
{
    public class Tts
    {
        /// <summary>
        /// 播放
        /// </summary>
        /// <param name="text"></param>
        public static void Play(string text)
        {
            text = OptionsSettings.SpliteLetterByRules(text);

            var result = TranslatorFactory.GetTranslator(TranslateType.Google).Translate(text, "auto", "zh-CN");
            var lang = result.SourceLanguage;

            //不能采用静态的,,因为如果改变了cache目录 静态的会有问题了
            string ttsCacheDir = Path.Combine(OptionsSettings.Settings.TranslateCachePath, "tts");
            var dir = Path.Combine(ttsCacheDir, lang);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            var md5 = Encrypts.CreateMd5EncryptFromString(text, Encoding.UTF8).Substring(8, 16);
            var mp3Path = Directory.GetFiles(dir, md5 + "*").FirstOrDefault();
            if (mp3Path == null)
            {
                mp3Path = Path.Combine(dir, md5 + "_" + DateTime.Now.ToString("yyyyMMddHHmmssff") + ".mp3");
                TextToSpeech.Text2Speech(text, lang, mp3Path);
                if (!File.Exists(mp3Path))
                {
                    return;
                }
            }
            ClsMci cm = new ClsMci { FileName = mp3Path };
            cm.play();
            //cm.StopT();
        }
    }
}