using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Identity.Contract;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Moq;
using Shared.Exceptions;
using Shared.Helpers;
using Shared.Interfaces;
using Shared.Models;
using Xunit;

namespace Shared.Tests.Helpers
{
    public class ExecutionContextHelperTests
    {
        private readonly ExecutionContextHelper _executionContextHelper;

        private const string ExpectedSecret = "TheCertificateSecret";

        public ExecutionContextHelperTests()
        {
            var authSettings = new AuthSettings
            {
                Secret = ExpectedSecret
            };

            var mockAuthSettingsOption = new Mock<IOptions<AuthSettings>>();
            mockAuthSettingsOption.SetupGet(option => option.Value).Returns(authSettings);
            _executionContextHelper = new ExecutionContextHelper(mockAuthSettingsOption.Object);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void FillExecutionContextNullOrEmptyJwtExpectArgumentNullException(string jwt)
        {
            Assert.Throws<ArgumentNullException>(() =>
                _executionContextHelper.FillExecutionContextFromJwt(jwt, new Mock<IExecutionContext>().Object));
        }

        [Fact]
        public void FillExecutionContextNullExecutionContextExpectArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
                _executionContextHelper.FillExecutionContextFromJwt("jwt", null));
        }

        [Fact]
        public void FillExecutionContextFromJwtValidTokenExpectInitialiseExecutionContext()
        {
            var mockExecutionContext = new Mock<IExecutionContext>();
            const string expectedUserId = "expectedUserId";
            const Role expectedRole = Role.User;

            var validJwt = GenerateToken(ExpectedSecret, new List<Claim>
            {
                new Claim(ClaimsConstants.IdClaimName,expectedUserId),
                new Claim(ClaimsConstants.RoleClaimName,expectedRole.ToString())
            });

            _executionContextHelper.FillExecutionContextFromJwt(validJwt, mockExecutionContext.Object);

            mockExecutionContext.Verify(context => context.Initialize(expectedRole, expectedUserId),
                Times.Once);
        }

        [Fact]
        public void FillExecutionContextFromJwtValidTokenLowercaseRoleExpectInitialiseExecutionContext()
        {
            var mockExecutionContext = new Mock<IExecutionContext>();
            const string expectedUserId = "expectedUserId";
            const Role expectedRole = Role.User;

            var validJwt = GenerateToken(ExpectedSecret, new List<Claim>
            {
                new Claim(ClaimsConstants.IdClaimName,expectedUserId),
                new Claim(ClaimsConstants.RoleClaimName,expectedRole.ToString().ToLower())
            });

            _executionContextHelper.FillExecutionContextFromJwt(validJwt, mockExecutionContext.Object);

            mockExecutionContext.Verify(context => context.Initialize(expectedRole, expectedUserId),
                Times.Once);
        }

        [Fact]
        public void FillExecutionContextJwtSignedInvalidCertificateExpectInvalidTokenException()
        {
            var mockExecutionContext = new Mock<IExecutionContext>();
            const string expectedUserId = "expectedUserId";
            const Role expectedRole = Role.User;

            var validJwt = GenerateToken("NotExpectedSecret", new List<Claim>
            {
                new Claim(ClaimsConstants.IdClaimName,expectedUserId),
                new Claim(ClaimsConstants.RoleClaimName,expectedRole.ToString().ToLower())
            });

            Assert.Throws<InvalidTokenException>(() =>
                _executionContextHelper.FillExecutionContextFromJwt(validJwt, mockExecutionContext.Object));

            mockExecutionContext.Verify(context => context.Initialize(It.IsAny<Role>(), It.IsAny<string>()),
                Times.Never);
        }

        [Fact]
        public void FillExecutionContextTokenWithoutUserIdExpectInvalidTokenException()
        {
            var mockExecutionContext = new Mock<IExecutionContext>();
            const Role expectedRole = Role.User;

            var validJwt = GenerateToken(ExpectedSecret, new List<Claim>
            {
                new Claim(ClaimsConstants.RoleClaimName,expectedRole.ToString().ToLower())
            });

            Assert.Throws<InvalidTokenException>(() =>
                _executionContextHelper.FillExecutionContextFromJwt(validJwt, mockExecutionContext.Object));

            mockExecutionContext.Verify(context => context.Initialize(It.IsAny<Role>(), It.IsAny<string>()),
                Times.Never);
        }

        [Fact]
        public void FillExecutionContextTokenWithoutRoleExpectInvalidTokenException()
        {
            var mockExecutionContext = new Mock<IExecutionContext>();
            const string expectedUserId = "expectedUserId";

            var validJwt = GenerateToken(ExpectedSecret, new List<Claim>
            {
                new Claim(ClaimsConstants.IdClaimName,expectedUserId)
            });

            Assert.Throws<InvalidTokenException>(() =>
                _executionContextHelper.FillExecutionContextFromJwt(validJwt, mockExecutionContext.Object));

            mockExecutionContext.Verify(context => context.Initialize(It.IsAny<Role>(), It.IsAny<string>()),
                Times.Never);
        }

        [Fact]
        public void FillExecutionContextTokenWithInvalidRoleExpectInvalidTokenException()
        {
            var mockExecutionContext = new Mock<IExecutionContext>();
            const string expectedUserId = "expectedUserId";

            var validJwt = GenerateToken(ExpectedSecret, new List<Claim>
            {
                new Claim(ClaimsConstants.IdClaimName,expectedUserId),
                new Claim(ClaimsConstants.RoleClaimName,"InvalidRole")
            });

            Assert.Throws<InvalidTokenException>(() =>
                _executionContextHelper.FillExecutionContextFromJwt(validJwt, mockExecutionContext.Object));

            mockExecutionContext.Verify(context => context.Initialize(It.IsAny<Role>(), It.IsAny<string>()),
                Times.Never);
        }

        // TODO: add tests for expired tokens

        private static string GenerateToken(string certificate, List<Claim> claims)
        {
            // generate token that is valid for an hour
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(certificate);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
