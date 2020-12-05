using Identity.Contract;

namespace Shared.Interfaces
{
    public interface IExecutionContext
    {
        Role UserRole { get; }

        string UserId { get; }

        bool IsInitialized { get; }

        void Initialize(Role userRole, string userId);

    }
}
