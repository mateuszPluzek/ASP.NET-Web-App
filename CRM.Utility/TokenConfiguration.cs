
namespace CRM.Utility
{
    internal class TokenConfiguration
    {
        public required string Subject { get; set; }
        public required string SecretKey { get; set; }
        public required string Issuer { get; set; }
        public required string Audience { get; set; }
        public required string TokenExpity { get; set; }
    }
}
