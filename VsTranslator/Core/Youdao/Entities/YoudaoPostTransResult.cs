using System.Collections.Generic;
using System.Runtime.Serialization;
using VsTranslator.Core.Youdao.Enums;

namespace VsTranslator.Core.Youdao.Entities
{
    [DataContract]
    public class YoudaoPostTransResult
    {
        [DataMember(Name = "type")]
        public string Type { get; set; }

        [DataMember(Name = "errorCode")]
        public ErrorCodes ErrorCode { get; set; }

        [DataMember(Name = "elapsedTime")]
        public int ElapsedTime { get; set; }

        [DataMember(Name = "translateResult")]
        public List<List<YoudaoTranslation>> TranslateResults { get; set; }
         
    }
    [DataContract]
    public class YoudaoTranslation
    {
        [DataMember(Name = "src")]
        public string Src  { get; set; }

        [DataMember(Name = "tgt")]
        public string Tgt { get; set; }
    }
}