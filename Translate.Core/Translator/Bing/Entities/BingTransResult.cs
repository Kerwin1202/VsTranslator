using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Translate.Core.Translator.Bing.Entities
{
    [DataContract]
    internal class BingTransResult
    {
        [DataMember]
        internal string From { get; set; }


        [DataMember]
        internal List<Translation> Translations { get; set; }
         
    }

    [DataContract]
    internal class Translation
    {
        [DataMember]
        internal int Count { get; set; }

        [DataMember]
        internal int MatchDegree { get; set; }

        [DataMember]
        internal string MatchedOriginalText { get; set; }

        [DataMember]
        internal int Rating { get; set; }

        [DataMember]
        internal string TranslatedText { get; set; }


    }
}