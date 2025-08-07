using FGC.Application.Common;
using FGC.Application.Auth.Models.Response;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Security.Authentication;
using FGC.Domain.Entities.Users;

namespace FGC.Application.Auth.Commands
{
    
    public class LoginCommand : IRequest<LoginResponse>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponse>
    {
        private readonly IApplicationDbContext _context;
         private readonly IConfiguration _configuration;

        public LoginCommandHandler(
            IApplicationDbContext context,
             IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var normalizedEmail = request.Email.Trim().ToLowerInvariant();

            var user = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email.Address.ToLower() == normalizedEmail, cancellationToken);

            if (user is null)
            {
                throw new AuthenticationException("Invalid credentials.");
            }

            if (!user.Active)
            {
                throw new AuthenticationException("Account is disabled.");
            }

            if (!user.Password.Challenge(request.Password))
            {
                throw new AuthenticationException("Invalid credentials.");
            }

            var (token, expiration) = GenerateJwtToken(user);

            return new LoginResponse
            {
                Token = token,
                Expiration = expiration
            };
        }

        private (string Token, DateTime Expiration) GenerateJwtToken(User user)
        {
            var secretKey = _configuration["JwtSettings:SecretKey"];
            if (string.IsNullOrWhiteSpace(secretKey))
                throw new InvalidOperationException("JWT SecretKey configuration is missing.");

            var key = Encoding.ASCII.GetBytes(secretKey);

            var claims = new List<Claim>
            {
                new ("username", user.Email.Address),
                new ("id", user.Id.ToString()),
                new (ClaimTypes.Role, user.TypeUser.ToString())
            };

            var expiration = DateTime.UtcNow.AddHours(2);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = expiration,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return (tokenHandler.WriteToken(token), expiration);
        }
    }
}
