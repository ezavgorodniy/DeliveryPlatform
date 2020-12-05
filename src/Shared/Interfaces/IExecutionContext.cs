using Identity.Contract;

namespace Shared.Interfaces
{
    public interface IExecutionContext
    {
        Role UserRole { get; }

        string UserId { get; }

        void Initialize(Role userRole, string userId);

    }
}
