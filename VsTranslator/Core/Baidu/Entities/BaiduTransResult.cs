using System.Collections.Generic;
using System.Runtime.Serialization;

namespace VsTranslator.Core.Baidu.Entities
{
    [DataContract]
    public class BaiduTransResult
    {
        [DataMember(Name = "from")]
        public string From { get; set; }
        [DataMember(Name = "to")]
        public string To { get; set; }
        [DataMember(Name = "trans_result")]
        public List<TransResult> TransResult { get; set; }
         
    }

    [DataContract]
    public class TransResult
    {
        [DataMember(Name = "src")]
        public string Src { get; set; }
        [DataMember(Name = "dst")]
        public string Dst { get; set; }

    }
}