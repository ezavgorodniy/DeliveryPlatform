using DeliveryPlatform.DataLayer.DataModels;
using Identity.Contract;

namespace DeliveryPlatform.Core.Interfaces
{
    public interface IPermissionChecker
    {
        bool RoleHasChangePermission(Role userRole, DeliveryState from, DeliveryState to);
    }
}
