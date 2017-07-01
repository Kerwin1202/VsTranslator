using System.Collections.Generic;
using System.Runtime.Serialization;
using Translate.Core.Translator.Youdao.Enums;

namespace Translate.Core.Translator.Youdao.Entities
{
    [DataContract]
    internal class YoudaoPostTransResult
    {
        [DataMember(Name = "type")]
        internal string Type { get; set; }

        [DataMember(Name = "errorCode")]
        internal ErrorCodes ErrorCode { get; set; }

        [DataMember(Name = "elapsedTime")]
        internal int ElapsedTime { get; set; }

        [DataMember(Name = "translateResult")]
        internal List<List<YoudaoTranslation>> TranslateResults { get; set; }
         
    }
    [DataContract]
    internal class YoudaoTranslation
    {
        [DataMember(Name = "src")]
        internal string Src  { get; set; }

        [DataMember(Name = "tgt")]
        internal string Tgt { get; set; }
    }
}