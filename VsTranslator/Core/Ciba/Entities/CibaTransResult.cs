using System.Collections.Generic;
using System.Runtime.Serialization;

namespace VsTranslator.Core.Ciba.Entities
{
    [DataContract]
    public class CibaTransResult
    {
        [DataMember(Name = "word_name")]
        public string WordName { get; set; }

        [DataMember(Name = "is_CRI")]
        public int IsCri { get; set; }

        [DataMember(Name = "exchange")]
        public WrodState Exchange { get; set; }

        [DataMember(Name = "symbols")]
        public Symbols Symbols { get; set; }

        [DataMember(Name = "items")]
        public List<string> Items { get; set; }

    }
    [DataContract]
    public class WrodState
    {
        [DataMember(Name = "word_pl")]
        public List<string> WordPlural { get; set; }

        [DataMember(Name = "word_past")]
        public List<string> WordPast { get; set; }

        [DataMember(Name = "word_done")]
        public List<string> WordDone { get; set; }

        [DataMember(Name = "word_ing")]
        public List<string> WordIng { get; set; }

        [DataMember(Name = "word_third")]
        public List<string> WordThird { get; set; }

        [DataMember(Name = "word_er")]
        public List<string> WordBetter { get; set; }

        [DataMember(Name = "word_est")]
        public List<string> WordBest { get; set; }
    }

    [DataContract]
    public class Symbols
    {
        [DataMember(Name = "ph_en")]
        public string PhoneticEnglish { get; set; }

        [DataMember(Name = "ph_am")]
        public string PhoneticAmerica { get; set; }

        [DataMember(Name = "ph_other")]
        public string PhoneticOther { get; set; }

        [DataMember(Name = "ph_en_mp3")]
        public string PhoneticEnglishMp3 { get; set; }

        [DataMember(Name = "ph_am_mp3")]
        public string PhoneticAmericaMp3 { get; set; }

        [DataMember(Name = "ph_tts_mp3")]
        public string PhoneticTextToSpeechMp3 { get; set; }

        [DataMember(Name = "parts")]
        public Parts Parts { get; set; }

    }

    [DataContract]
    public class Parts
    {
        [DataMember(Name = "part")]
        public string Part { get; set; }

        [DataMember(Name = "means")]
        public List<string> Means { get; set; }
    }
}