using System.Runtime.Serialization;

namespace Translate.Core.Translator.Bing.Entities
{
    [DataContract]
    internal class AdmAccessToken
    {
        [DataMember(Name = "access_token")]
        internal string AccessToken { get; set; }
        [DataMember(Name = "token_type")]
        internal string TokenType { get; set; }
        [DataMember(Name = "expires_in")]
        internal string ExpiresIn { get; set; }
        [DataMember(Name = "scope")]
        internal string Scope { get; set; }
    }
}