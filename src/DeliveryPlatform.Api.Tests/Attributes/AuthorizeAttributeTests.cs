using System.Collections.Generic;
using DeliveryPlatform.Attributes;
using Identity.Contract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Shared.Interfaces;
using Xunit;

namespace DeliveryPlatform.Api.Tests.Attributes
{
    public class AuthorizeAttributeTests
    {
        private readonly AuthorizationFilterContext _authContext;
        private readonly Mock<IExecutionContext> _mockExecutionContext; 

        public AuthorizeAttributeTests()
        {
            var mockHttpContext = new Mock<HttpContext>();

            var fakeActionContext =
                new ActionContext(mockHttpContext.Object,
                    new Microsoft.AspNetCore.Routing.RouteData(),
                    new Microsoft.AspNetCore.Mvc.Abstractions.ActionDescriptor());

            _authContext = new AuthorizationFilterContext(fakeActionContext, new List<IFilterMetadata>());

            _mockExecutionContext = new Mock<IExecutionContext>();

            var serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton(provider => _mockExecutionContext.Object);
            var serviceProvider = serviceCollection.BuildServiceProvider();

            mockHttpContext.SetupGet(context => context.RequestServices).Returns(serviceProvider);

            _authContext.HttpContext = mockHttpContext.Object;
        }

        [Fact]
        public void AuthorizeExecutionContextIsNotInitializedExpectUnauthorized()
        {
            _mockExecutionContext.SetupGet(executionContext => executionContext.IsInitialized).Returns(false);

            var attr = new AuthorizeAttribute(Role.User);

            attr.OnAuthorization(_authContext);

            Assert.IsType<JsonResult>(_authContext.Result);

            var result = (JsonResult) _authContext.Result;
            Assert.Equal(StatusCodes.Status401Unauthorized, result.StatusCode);
        }

        [Theory]
        [InlineData(Role.Partner, Role.User)]
        [InlineData(Role.User, Role.Partner)]
        public void AuthorizeRolesNotMatchExpectUnauthorized(Role expectedRole, Role actualRole)
        {
            _mockExecutionContext.SetupGet(executionContext => executionContext.IsInitialized).Returns(true);
            _mockExecutionContext.SetupGet(executionContext => executionContext.UserRole).Returns(actualRole);

            var attr = new AuthorizeAttribute(expectedRole);

            attr.OnAuthorization(_authContext);

            Assert.IsType<JsonResult>(_authContext.Result);

            var result = (JsonResult)_authContext.Result;
            Assert.Equal(StatusCodes.Status401Unauthorized, result.StatusCode);
        }

        [Theory]
        [InlineData(Role.Partner | Role.User, Role.Partner)]
        [InlineData(Role.Partner | Role.User, Role.User)]
        [InlineData(Role.Partner, Role.Partner)]
        [InlineData(Role.User, Role.User)]
        public void AuthorizeRolesMatchExpectNotTouchResult(Role expectedRole, Role actualRole)
        {
            _mockExecutionContext.SetupGet(executionContext => executionContext.IsInitialized).Returns(true);
            _mockExecutionContext.SetupGet(executionContext => executionContext.UserRole).Returns(actualRole);

            var attr = new AuthorizeAttribute(expectedRole);

            attr.OnAuthorization(_authContext);

            Assert.Null(_authContext.Result);
        }
    }
}
