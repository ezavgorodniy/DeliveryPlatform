using System;
using System.Threading.Tasks;
using Identity.Contract;
using Identity.Core.Models;
using Identity.Core.Services;
using Identity.DataLayer.DataModels;
using Identity.DataLayer.Interfaces;
using Microsoft.Extensions.Options;
using Moq;
using Shared.Models;
using Xunit;

namespace Identity.Core.Tests.Services
{
    public class AuthenticationServiceTests
    {
        private readonly AuthenticationService _authenticationService;

        private readonly Mock<IUsersRepo> _mockUsersRepo;
        private const string ExpectedSecret = "TheCertificateSecret";

        public AuthenticationServiceTests()
        {
            var authSettings = new AuthSettings
            {
                Secret = ExpectedSecret
            };
            _mockUsersRepo = new Mock<IUsersRepo>();

            var mockAuthSettingsOption = new Mock<IOptions<AuthSettings>>();
            mockAuthSettingsOption.SetupGet(option => option.Value).Returns(authSettings);
            
            _authenticationService = new AuthenticationService(_mockUsersRepo.Object,
                mockAuthSettingsOption.Object);
        }

        [Fact]
        public async Task AuthenticateNullDtoExpectArgumentNullException()
        {
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await _authenticationService.Authenticate(null));
        }

        [Fact]
        public async Task AuthenticateRepoReturnedNullExepctNullToken()
        {
            const string expectedUserName = "expectedUserName";
            const string expectedUserPassword = "expectedUserPassword";

            _mockUsersRepo.Setup(repo => repo.Authenticate(expectedUserName, expectedUserPassword))
                .Returns(Task.FromResult(null as User));

            var actualResult = await _authenticationService.Authenticate(new AuthenticateRequestDto
            {
                Password = expectedUserPassword,
                Username = expectedUserName
            });

            Assert.Null(actualResult);
        }

        [Fact]
        public async Task AuthenticateRepoReturnedUserExpectJwt()
        {
            const string expectedUserName = "expectedUserName";
            const string expectedUserPassword = "expectedUserPassword";

            const string expectedId = "expectedId";
            const Role expectedRole = Role.User;

            _mockUsersRepo.Setup(repo => repo.Authenticate(expectedUserName, expectedUserPassword))
                .Returns(Task.FromResult(new User
                {
                    Id = expectedId,
                    Password = expectedUserPassword,
                    Role = expectedRole
                }));

            var actualResult = await _authenticationService.Authenticate(new AuthenticateRequestDto
            {
                Password = expectedUserPassword,
                Username = expectedUserName
            });

            Assert.NotNull(actualResult);
            Assert.NotNull(actualResult.Token);

            // TODO: decode the JWT and verify that it contain id and role claim
        }
    }
}
