using DeliveryPlatform.Core.Helpers;
using DeliveryPlatform.DataLayer.DataModels;
using Identity.Contract;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace DeliveryPlatform.Core.Tests.Helpers
{
    public class PermissionCheckerTests
    {
        private readonly PermissionChecker _permissionChecker;

        public PermissionCheckerTests()
        {
            var mockLogger = new Mock<ILogger<PermissionChecker>>();

            _permissionChecker = new PermissionChecker(mockLogger.Object);
        }

        // from requirements: "Users may approve a delivery before it starts"
        [Fact]
        public void UserMayAppoveDeliveryBeforeItStarts()
        {
            var actual = _permissionChecker.RoleHasChangePermission(Role.User,
                DeliveryState.Created, DeliveryState.Approved);

            Assert.True(actual);
        }

        [Fact]
        public void PartnerMayNotAppoveDeliveryBeforeItStarts()
        {
            var actual = _permissionChecker.RoleHasChangePermission(Role.Partner,
                DeliveryState.Created, DeliveryState.Approved);

            Assert.False(actual);
        }

        [Theory]
        [InlineData(DeliveryState.Approved)]
        [InlineData(DeliveryState.Cancelled)]
        [InlineData(DeliveryState.Completed)]
        [InlineData(DeliveryState.Expired)]
        public void UserMayNotAppoveDeliveryAfterItStarts(DeliveryState previousState)
        {
            var actual = _permissionChecker.RoleHasChangePermission(Role.User,
                previousState, DeliveryState.Approved);

            Assert.False(actual);
        }

        // from requirements: "Partner may complete a delivery, that is already in approved state."
        [Fact]
        public void PartnerMayCompleteDeliveryInApprovedState()
        {
            var actual = _permissionChecker.RoleHasChangePermission(Role.Partner,
                DeliveryState.Approved, DeliveryState.Completed);

            Assert.True(actual);
        }

        [Fact]
        public void UserMayNotCompleteDeliveryInApprovedState()
        {
            var actual = _permissionChecker.RoleHasChangePermission(Role.User,
                DeliveryState.Approved, DeliveryState.Completed);

            Assert.False(actual);
        }

        [Theory]
        [InlineData(DeliveryState.Created)]
        [InlineData(DeliveryState.Cancelled)]
        [InlineData(DeliveryState.Completed)]
        [InlineData(DeliveryState.Expired)]
        public void PartnerMayNotCompleteDeliveryInNotApprovedState(DeliveryState previousState)
        {
            var actual = _permissionChecker.RoleHasChangePermission(Role.Partner,
                previousState, DeliveryState.Completed);

            Assert.False(actual);
        }

        // from requirements: Either the partner or the user should be able to cancel a pending delivery (in statecreated or approved ).

        [Theory]
        [InlineData(Role.Partner, DeliveryState.Approved)]
        [InlineData(Role.Partner, DeliveryState.Created)]
        [InlineData(Role.User, DeliveryState.Approved)]
        [InlineData(Role.User, DeliveryState.Created)]
        public void PartnerOrUserMayCancelledPendingDelivery(Role role, DeliveryState previousState)
        {
            var actual = _permissionChecker.RoleHasChangePermission(role,
                previousState, DeliveryState.Cancelled);

            Assert.True(actual);
        }

        // assumption: nobody can change state of expired, cancelled or completed delivery

        [Theory]
        [InlineData(Role.Partner, DeliveryState.Cancelled)]
        [InlineData(Role.Partner, DeliveryState.Completed)]
        [InlineData(Role.Partner, DeliveryState.Expired)]
        [InlineData(Role.User, DeliveryState.Cancelled)]
        [InlineData(Role.User, DeliveryState.Completed)]
        [InlineData(Role.User, DeliveryState.Expired)]
        public void NobodyCanChangeClosedDelivery(Role role, DeliveryState previousState)
        {
            var actual = _permissionChecker.RoleHasChangePermission(role,
                previousState, DeliveryState.Approved);

            Assert.False(actual);
        }

        // assumption creation and expiration out of scope of this checker

        [Theory]
        [InlineData(Role.Partner, DeliveryState.Created)]
        [InlineData(Role.Partner, DeliveryState.Expired)]
        [InlineData(Role.User, DeliveryState.Created)]
        [InlineData(Role.User, DeliveryState.Expired)]
        public void CreationExpirationOutOfScopeOfThisChecker(Role role, DeliveryState newState)
        {
            var actual = _permissionChecker.RoleHasChangePermission(role,
                DeliveryState.Created, newState);

            Assert.False(actual);
        }
    }
}
