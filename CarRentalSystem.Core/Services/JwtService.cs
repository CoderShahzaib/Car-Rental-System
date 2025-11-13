using CarRentalSystem.Core.DTOs;
using CarRentalSystem.Core.Identity;
using CarRentalSystem.Core.ServiceContracts;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace CarRentalSystem.Services
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _configuration;

        public JwtService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public AuthenticationResponse CreateJwtToken(ApplicationUser user)
        {
            // Read expiration from config
            double expirationMinutes = Convert.ToDouble(_configuration["Jwt:EXPIRATION_MINUTES"]);
            DateTime expiration = DateTime.UtcNow.AddMinutes(expirationMinutes);

            // Create claims
            Claim[] claims = new Claim[]
            {
        new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
        new Claim(ClaimTypes.Name, user.PersonName)
            };

            // Security key
            var securityKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])
            );

            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // Generate token
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: expiration,
                signingCredentials: signingCredentials
            );

            string tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            // Return DTO
            return new AuthenticationResponse
            {
                Token = tokenString,
                PersonName = user.PersonName,
                Expiration = expiration,
                RefreshToken = GenerateRefreshToken(),
                RefreshTokenExpiration = DateTime.Now.AddMinutes(Convert.ToInt32(_configuration["RefreshToken:EXPIRATION_MINUTES"]))
            };
        }
    
        private string GenerateRefreshToken()
        {
            byte[] bytes = new byte[64];
            var randomNumberGenerator = RandomNumberGenerator.Create();
            randomNumberGenerator.GetBytes(bytes);
            return Convert.ToBase64String(bytes);
        }
    }
}
