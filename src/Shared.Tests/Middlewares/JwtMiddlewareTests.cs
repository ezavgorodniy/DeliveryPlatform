using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using Shared.Interfaces;
using Shared.Middlewares;
using Xunit;

namespace Shared.Tests.Middlewares
{
    public class JwtMiddlewareTests
    {
        private readonly JwtMiddleware _middleware;
        private bool _nextWasCalled;

        private Mock<IExecutionContext> _mockExecutionContext;
        private Mock<IExecutionContextHelper> _mockExecutionContextHelper;
        private Mock<HttpContext> _mockHttpContext;
        private Mock<HttpRequest> _mockHttpRequest;
        private HeaderDictionary _headers;

        public JwtMiddlewareTests()
        {
            _headers = new HeaderDictionary();
            
            _mockHttpRequest = new Mock<HttpRequest>();
            _mockHttpRequest.SetupGet(request => request.Headers).Returns(_headers);

            _mockHttpContext = new Mock<HttpContext>();
            _mockHttpContext.SetupGet(context => context.Request).Returns(_mockHttpRequest.Object);

            _mockExecutionContext = new Mock<IExecutionContext>();
            _mockExecutionContextHelper = new Mock<IExecutionContextHelper>();

            var logger = new Mock<ILogger<JwtMiddleware>>();

            _nextWasCalled = false;

            _middleware = new JwtMiddleware(context =>
            {
                _nextWasCalled = true;
                return Task.FromResult(0);
            }, logger.Object);
        }

        [Fact]
        public async Task InvokeNoHeadersShouldCallNext()
        {
            await _middleware.Invoke(_mockHttpContext.Object,
                _mockExecutionContext.Object,
                _mockExecutionContextHelper.Object);

            Assert.True(_nextWasCalled);
            _mockExecutionContextHelper.Verify(
                helper => helper.FillExecutionContextFromJwt(It.IsAny<string>(), It.IsAny<IExecutionContext>()),
                Times.Never);
        }

        [Fact]
        public async Task InvokeAuthorizationHeadersNotStartWithBearerShouldCallNext()
        {
            _headers["Authorization"] = "justJwt";

            await _middleware.Invoke(_mockHttpContext.Object,
                _mockExecutionContext.Object,
                _mockExecutionContextHelper.Object);

            Assert.True(_nextWasCalled);
            _mockExecutionContextHelper.Verify(
                helper => helper.FillExecutionContextFromJwt(It.IsAny<string>(), It.IsAny<IExecutionContext>()),
                Times.Never);
        }

        [Fact]
        public async Task InvokeAuthorizationHeadersBearerTokenShouldFillContextAndCallNext()
        {
            const string expectedJwt = "expectedJwt";
            _headers["Authorization"] = $"Bearer  {expectedJwt}";

            await _middleware.Invoke(_mockHttpContext.Object,
                _mockExecutionContext.Object,
                _mockExecutionContextHelper.Object);

            Assert.True(_nextWasCalled);
            _mockExecutionContextHelper.Verify(
                helper => helper.FillExecutionContextFromJwt(expectedJwt, _mockExecutionContext.Object),
                Times.Once);
        }

        [Fact]
        public async Task InvokeAuthorizationHeadersExceptionHappenedShoulCallNext()
        {
            const string expectedJwt = "expectedJwt";
            _headers["Authorization"] = $"Bearer  {expectedJwt}";


            _mockExecutionContextHelper
                .Setup(helper => helper.FillExecutionContextFromJwt(expectedJwt, _mockExecutionContext.Object))
                .Throws<Exception>();

            await _middleware.Invoke(_mockHttpContext.Object,
                _mockExecutionContext.Object,
                _mockExecutionContextHelper.Object);

            Assert.True(_nextWasCalled);
        }
    }
}
