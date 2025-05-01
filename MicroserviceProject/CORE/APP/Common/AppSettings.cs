using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace CORE.APP.Common
{
    public static class AppSettings
    {
        public static string Issuer = "ScanBookIssuer";
        public static string Audience = "ScanBookAudience";
        public static string Key = "SuperSecretKeyForJwtSigning2025!";
        public static SecurityKey SigningKey => new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Key));
    }

}