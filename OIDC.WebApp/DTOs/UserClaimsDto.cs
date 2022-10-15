using OIDC.ClassLib.Abstracts;

namespace OIDC.WebApp.DTOs
{
    public record UserClaimsDto : CustomDtoBase
    {
        public string Issuer { get; set; }
        public string OriginalIssuer { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }
        public string ValueType { get; set; }
    }
}
