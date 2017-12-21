using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Translate.Core.Translator.Bing.Entities.Post
{
    [DataContract]
    public class BingPostTransParams
    {
        [JsonProperty("id"), DataMember(Name = "id")]
        public long Id { get; set; } = Convert.ToInt64(DateTime.Now.ToString("yyMMddHHmmssffff"));

        [JsonProperty("text"), DataMember(Name = "text")]
        public string Text { get; set; }
    }
}