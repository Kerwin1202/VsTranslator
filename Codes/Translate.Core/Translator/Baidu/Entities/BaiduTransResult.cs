using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Translate.Core.Translator.Baidu.Entities
{
    [DataContract]
    internal class BaiduTransResult
    {
        [DataMember(Name = "from")]
        internal string From { get; set; }
        [DataMember(Name = "to")]
        internal string To { get; set; }
        [DataMember(Name = "trans_result")]
        internal List<TransResult> TransResult { get; set; }
         
    }

    [DataContract]
    internal class TransResult
    {
        [DataMember(Name = "src")]
        internal string Src { get; set; }
        [DataMember(Name = "dst")]
        internal string Dst { get; set; }

    }
}