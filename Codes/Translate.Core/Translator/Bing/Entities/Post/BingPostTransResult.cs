using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Translate.Core.Translator.Bing.Entities.Post
{
    [DataContract]
    public class BingPostTransResult
    {
        [DataMember(Name = "from")]
        public string From { get; set; }

        [DataMember(Name = "to")]
        public string To { get; set; }

        [DataMember(Name = "items")]
        public List<BingPostTransResultItem> Items { get; set; }
    }

    [DataContract]
    public class BingPostTransResultItem : BingPostTransParams
    {
        [DataMember(Name = "wordAlignment")]
        public string WordAlignment { get; set; }
    }
}