using System.Collections.Generic;
using System.Runtime.Serialization;
using VsTranslator.Core.Translator.Youdao.Enums;

namespace VsTranslator.Core.Translator.Youdao.Entities
{
    [DataContract]
    public class YoudaoTransResult
    {
        [DataMember(Name = "errorCode")]
        public ErrorCodes ErrorCode { get; set; }

        [DataMember(Name = "query")]
        public string Query { get; set; }

        [DataMember(Name = "translation")]
        public List<string> Translation { get; set; }

        [DataMember(Name = "basic")]
        public YoudaoBaseResult BasicResult { get; set; }

        [DataMember(Name = "web")]
        public List<YoudaoWebResult> WebResult { get; set; }
    }
    [DataContract]
    public class YoudaoBaseResult
    {
        [DataMember(Name = "phonetic")]
        public string Phonetic { get; set; }

        [DataMember(Name = "uk-phonetic")]
        public string UkPhonetic { get; set; }

        [DataMember(Name = "us-phonetic")]
        public string UsPhonetic { get; set; }

        [DataMember(Name = "explains")]
        public List<string> Explains { get; set; }
    }
    [DataContract]
    public class YoudaoWebResult
    {
        [DataMember(Name = "key")]
        public string Key { get; set; }
        [DataMember(Name = "value")]
        public List<string> Value { get; set; }
    }
}