using System.Collections.Generic;
using System.Runtime.Serialization;
using Translate.Core.Translator.Youdao.Enums;

namespace Translate.Core.Translator.Youdao.Entities
{
    [DataContract]
    internal class YoudaoTransResult
    {
        [DataMember(Name = "errorCode")]
        internal ErrorCodes ErrorCode { get; set; }

        [DataMember(Name = "query")]
        internal string Query { get; set; }

        [DataMember(Name = "translation")]
        internal List<string> Translation { get; set; }

        [DataMember(Name = "basic")]
        internal YoudaoBaseResult BasicResult { get; set; }

        [DataMember(Name = "web")]
        internal List<YoudaoWebResult> WebResult { get; set; }
    }
    [DataContract]
    internal class YoudaoBaseResult
    {
        [DataMember(Name = "phonetic")]
        internal string Phonetic { get; set; }

        [DataMember(Name = "uk-phonetic")]
        internal string UkPhonetic { get; set; }

        [DataMember(Name = "us-phonetic")]
        internal string UsPhonetic { get; set; }

        [DataMember(Name = "explains")]
        internal List<string> Explains { get; set; }
    }
    [DataContract]
    internal class YoudaoWebResult
    {
        [DataMember(Name = "key")]
        internal string Key { get; set; }
        [DataMember(Name = "value")]
        internal List<string> Value { get; set; }
    }
}