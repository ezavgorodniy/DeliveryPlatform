using System;
using Identity.Contract;
using Xunit;

namespace Shared.Tests
{
    public class ExecutionContextTests
    {
        private readonly ExecutionContext _executionContext;

        public ExecutionContextTests()
        {
            _executionContext = new ExecutionContext();
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void InitializeWithNullOrEmptyUserIdExpectArgumentNullException(string userId)
        {
            Assert.Throws<ArgumentNullException>(() => _executionContext.Initialize(Role.Partner, userId));
        }

        [Fact]
        public void InitializeExpectInitializeRoleAndUserId()
        {
            const Role expectedRole = Role.User;
            const string expectedUserId = "expectedUserId";

            _executionContext.Initialize(expectedRole, expectedUserId);

            Assert.Equal(expectedUserId, _executionContext.UserId);
            Assert.Equal(expectedRole, _executionContext.UserRole);
        }

        [Fact]
        public void InitializeTwiceExpectException()
        {
            _executionContext.Initialize(Role.Partner, "userId");
            Assert.Throws<Exception>(() =>
            {
                _executionContext.Initialize(Role.Partner, "userId");
            });
        }
    }
}

