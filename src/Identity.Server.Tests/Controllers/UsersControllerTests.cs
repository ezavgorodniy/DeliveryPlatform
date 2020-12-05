using System.Threading.Tasks;
using Identity.Core.Interfaces;
using Identity.Core.Models;
using Identity.Server.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Identity.Server.Tests.Controllers
{
    public class UsersControllerTests
    {
        private readonly UsersController _usersController;
        private readonly Mock<IAuthenticationService> _mockAuthenticationService;

        public UsersControllerTests()
        {
            var mockLogger = new Mock<ILogger<UsersController>>();
            _mockAuthenticationService = new Mock<IAuthenticationService>();

            _usersController = new UsersController(mockLogger.Object, _mockAuthenticationService.Object);
        }

        [Fact]
        public async Task AuthenticateNullDtoExpectBadRequest()
        {
            var actualResult = await _usersController.Authenticate(null);

            Assert.IsType<BadRequestResult>(actualResult);
        }

        [Fact]
        public async Task AuthenticateServiceReturnedNullExpectBadRequest()
        {
            var request = new AuthenticateRequestDto
            {
                Password = "incorrectPassword",
                Username = "user"
            };

            _mockAuthenticationService.Setup(service => service.Authenticate(request))
                .Returns(Task.FromResult(null as AuthenticateResponse));

            var actualResult = await _usersController.Authenticate(request);

            Assert.IsType<BadRequestResult>(actualResult);
        }

        [Fact]
        public async Task AuthenticateServiceReturnedToken()
        {
            var request = new AuthenticateRequestDto
            {
                Password = "incorrectPassword",
                Username = "user"
            };

            const string expectedResponseToken = "someJwt";
            var expectedResponse = new AuthenticateResponse
            {
                Token = expectedResponseToken
            };

            _mockAuthenticationService.Setup(service => service.Authenticate(request))
                .Returns(Task.FromResult(expectedResponse));

            var actualResult = await _usersController.Authenticate(request);

            Assert.IsType<OkObjectResult>(actualResult);

            var okResult = (OkObjectResult) actualResult;
            Assert.IsType<AuthenticateResponse>(okResult.Value);

            var okResultObject = (AuthenticateResponse) okResult.Value;
            Assert.Equal(expectedResponseToken, okResultObject.Token);
        }

    }
}
