using System.Runtime.Serialization;

namespace VsTranslator.Core.Translator.Bing.Entities
{
    [DataContract]
    public class AdmAccessToken
    {
        [DataMember(Name = "access_token")]
        public string AccessToken { get; set; }
        [DataMember(Name = "token_type")]
        public string TokenType { get; set; }
        [DataMember(Name = "expires_in")]
        public string ExpiresIn { get; set; }
        [DataMember(Name = "scope")]
        public string Scope { get; set; }
    }
}