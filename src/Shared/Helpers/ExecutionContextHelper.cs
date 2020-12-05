using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using Identity.Contract;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Shared.Exceptions;
using Shared.Interfaces;
using Shared.Models;
using Exception = System.Exception;

namespace Shared.Helpers
{
    internal class ExecutionContextHelper: IExecutionContextHelper
    {
        private readonly AuthSettings _authSettings;

        public ExecutionContextHelper(IOptions<AuthSettings> options)
        {
            _authSettings = options.Value;
        }

        public void FillExecutionContextFromJwt(string jwt, IExecutionContext executionContext)
        {
            if (string.IsNullOrEmpty(jwt))
            {
                throw new ArgumentNullException(nameof(jwt));
            }

            if (executionContext == null)
            {
                throw new ArgumentNullException(nameof(executionContext));
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_authSettings.Secret);
            SecurityToken validatedToken;
            try
            {
                tokenHandler.ValidateToken(jwt, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                }, out validatedToken);
            }
            catch (Exception e)
            {
                throw new InvalidTokenException("Unable to validate token", e);
            }

            var jwtToken = (JwtSecurityToken)validatedToken;
            var userId = ReadRequiredClaim(jwtToken, ClaimsConstants.IdClaimName);
            if (!Enum.TryParse<Role>(ReadRequiredClaim(jwtToken, ClaimsConstants.RoleClaimName), true, out var role))
            {
                throw new InvalidTokenException("Invalid role claim");
            }

            executionContext.Initialize(role, userId);
        }

        private static string ReadRequiredClaim(JwtSecurityToken jwtToken, string claimName)
        {
            var claim = jwtToken.Claims.FirstOrDefault(x => x.Type == claimName);
            if (claim == null)
            {
                throw new InvalidTokenException($"Missed required claim with name {claimName}");
            }

            return claim.Value;
        }
    }
}
