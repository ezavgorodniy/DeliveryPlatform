using DeliveryPlatform.Core.Interfaces;
using DeliveryPlatform.DataLayer.DataModels;
using Identity.Contract;
using Microsoft.Extensions.Logging;

namespace DeliveryPlatform.Core.Helpers
{
    public class PermissionChecker : IPermissionChecker
    {
        private readonly ILogger<PermissionChecker> _logger;

        public PermissionChecker(ILogger<PermissionChecker> logger)
        {
            _logger = logger;
        }

        public bool RoleHasChangePermission(Role userRole, DeliveryState from, DeliveryState to)
        {

            if (from == DeliveryState.Completed ||
                from == DeliveryState.Cancelled ||
                from == DeliveryState.Expired)
            {
                // nobody can change delivery if it's finished
                return LogAndReturnValue(userRole, from, to, false);
            }

            if (to == DeliveryState.Expired || to == DeliveryState.Created)
            {
                // this is automatic and controlled outside of scope of this checker
                return LogAndReturnValue(userRole, from, to, false);
            }

            if (from == DeliveryState.Created && to == DeliveryState.Approved)
            {
                // only user may approve the delivery
                return LogAndReturnValue(userRole, from, to, userRole == Role.User);
            }

            if (to == DeliveryState.Completed)
            {
                // Partner may complete a delivery, that is already in approved state.
                return LogAndReturnValue(userRole, from, to,
                    from == DeliveryState.Approved && userRole == Role.Partner);
            }

            if (to == DeliveryState.Cancelled)
            {
                // Either the partner or the user should be able to cancel a pending delivery (in statecreated or approved ).
                return LogAndReturnValue(userRole, from, to,
                    from == DeliveryState.Approved || from == DeliveryState.Created);
            }

            // other operations are forbidden by default
            return false;
        }

        private bool LogAndReturnValue(Role userRole, DeliveryState from, DeliveryState to, bool result)
        {
            var operationPrefix = $"RoleHasChangePermission call for role {userRole} from {from} to {to}";
            _logger.LogTrace($"{operationPrefix} is {(result ? "allowed" : "forbidden")}");
            return result;
        }
    }
}
