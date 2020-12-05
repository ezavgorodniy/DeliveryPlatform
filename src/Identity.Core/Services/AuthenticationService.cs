using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Identity.Contract;
using Identity.Core.Interfaces;
using Identity.Core.Models;
using Identity.DataLayer.DataModels;
using Identity.DataLayer.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Identity.Core.Services
{
    internal class AuthenticationService : IAuthenticationService
    {
        private readonly IUsersRepo _usersRepo;

        private readonly AuthSettings _authSettings;

        public AuthenticationService(IUsersRepo usersRepo,
            IOptions<AuthSettings> authSettings)
        {
            _usersRepo = usersRepo;
            _authSettings = authSettings.Value;
        }

        public async Task<AuthenticateResponse> Authenticate(AuthenticateRequestDto authenticationRequestDto)
        {
            if (authenticationRequestDto == null)
            {
                throw new ArgumentNullException(nameof(authenticationRequestDto));
            }

            var user = await _usersRepo.Authenticate(authenticationRequestDto.Username,
                authenticationRequestDto.Password);
            return user == null
                ? null
                : new AuthenticateResponse
                {
                    Token = GenerateJwtToken(user)
                };
        }

        // TODO: inspired by https://jasonwatmore.com/post/2019/10/11/aspnet-core-3-jwt-authentication-tutorial-with-example-api
        // better to check if security algorithms used here is the best
        private string GenerateJwtToken(User user)
        {
            // generate token that is valid for an hour
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_authSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimsConstants.IdClaimName, user.Id),
                    new Claim(ClaimsConstants.RoleClaimName, user.Role.ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
