using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Translate.Core.Translator.Bing.Entities
{
    [DataContract]
    public class BingTransResult
    {
        [DataMember]
        public string From { get; set; }


        [DataMember]
        public List<Translation> Translations { get; set; }
         
    }

    [DataContract]
    public class Translation
    {
        [DataMember]
        public int Count { get; set; }

        [DataMember]
        public int MatchDegree { get; set; }

        [DataMember]
        public string MatchedOriginalText { get; set; }

        [DataMember]
        public int Rating { get; set; }

        [DataMember]
        public string TranslatedText { get; set; }


    }
}