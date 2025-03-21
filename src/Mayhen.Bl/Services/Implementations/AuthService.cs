using Mayhem.Configuration.Interfaces;
using Mayhem.Dal.Dto.Dtos;
using Mayhem.Dal.Interfaces.Repositories;
using Mayhen.Bl.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Mayhen.Bl.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly IMayhemConfigurationService mayhemConfigurationService;
        private readonly IUserRepository userRepository;


        public AuthService(IMayhemConfigurationService mayhemConfigurationService, IUserRepository userRepository)
        {
            this.mayhemConfigurationService = mayhemConfigurationService;
            this.userRepository = userRepository;
        }

        public async Task<string> CreateTokenAsync(string walletAddress)
        {
            ApplicationUserDto user = await userRepository.GetApplicationUserByWalletAsync(walletAddress);

            if (user != null)
            {
                return GetToken(user);
            }

            return string.Empty;
        }

        public async Task<string> RefreshToken(int userId)
        {
            ApplicationUserDto user = await userRepository.GetApplicationUserByIdAsync(userId);
            if (user != null)
            {
                return GetToken(user);
            }

            return string.Empty;
        }

        private string GetToken(ApplicationUserDto user)
        {
            List<Claim> authClaims = new()
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            };

            SymmetricSecurityKey authSigningKey = new(Encoding.UTF8.GetBytes(mayhemConfigurationService.MayhemConfiguration.ServiceSecretsConfigruation.JwtKey));

            JwtSecurityToken token = new(
                issuer: mayhemConfigurationService.MayhemConfiguration.ServiceSecretsConfigruation.JwtIssuer,
                audience: mayhemConfigurationService.MayhemConfiguration.ServiceSecretsConfigruation.JwtAudience,
                expires: DateTime.UtcNow.AddMinutes(mayhemConfigurationService.MayhemConfiguration.CommonConfiguration.TokenLifetimeInMinutes),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
