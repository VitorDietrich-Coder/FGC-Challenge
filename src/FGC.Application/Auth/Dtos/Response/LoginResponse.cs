using System.Diagnostics.CodeAnalysis;

namespace FGC.Application.Auth.Models.Response
{
    
    public record LoginResponse
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
    }
}
