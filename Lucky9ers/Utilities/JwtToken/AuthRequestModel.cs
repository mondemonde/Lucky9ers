using System;

namespace api.Utilities.JwtToken
{
    namespace GetToken
    {
        public class AuthRequestModel
        {
            public Guid APIKey { get; set; }
            public string CreationDate { get; set; }
            public string Signature { get; set; }
            public string grant_type { get; set; }
            public string Role { get; set; }
        }
    }
}