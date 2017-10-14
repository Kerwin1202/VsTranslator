using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Translate.Core.Translator.Ciba.Entities
{
    [DataContract]
    internal class CibaTransResult
    {
        [DataMember(Name = "word_name")]
        internal string WordName { get; set; }

        [DataMember(Name = "is_CRI")]
        internal int IsCri { get; set; }

        [DataMember(Name = "exchange")]
        internal WrodState Exchange { get; set; }

        [DataMember(Name = "symbols")]
        internal Symbols Symbols { get; set; }

        [DataMember(Name = "items")]
        internal List<string> Items { get; set; }

    }
    [DataContract]
    internal class WrodState
    {
        [DataMember(Name = "word_pl")]
        internal List<string> WordPlural { get; set; }

        [DataMember(Name = "word_past")]
        internal List<string> WordPast { get; set; }

        [DataMember(Name = "word_done")]
        internal List<string> WordDone { get; set; }

        [DataMember(Name = "word_ing")]
        internal List<string> WordIng { get; set; }

        [DataMember(Name = "word_third")]
        internal List<string> WordThird { get; set; }

        [DataMember(Name = "word_er")]
        internal List<string> WordBetter { get; set; }

        [DataMember(Name = "word_est")]
        internal List<string> WordBest { get; set; }
    }

    [DataContract]
    internal class Symbols
    {
        [DataMember(Name = "ph_en")]
        internal string PhoneticEnglish { get; set; }

        [DataMember(Name = "ph_am")]
        internal string PhoneticAmerica { get; set; }

        [DataMember(Name = "ph_other")]
        internal string PhoneticOther { get; set; }

        [DataMember(Name = "ph_en_mp3")]
        internal string PhoneticEnglishMp3 { get; set; }

        [DataMember(Name = "ph_am_mp3")]
        internal string PhoneticAmericaMp3 { get; set; }

        [DataMember(Name = "ph_tts_mp3")]
        internal string PhoneticTextToSpeechMp3 { get; set; }

        [DataMember(Name = "parts")]
        internal Parts Parts { get; set; }

    }

    [DataContract]
    internal class Parts
    {
        [DataMember(Name = "part")]
        internal string Part { get; set; }

        [DataMember(Name = "means")]
        internal List<string> Means { get; set; }
    }
}