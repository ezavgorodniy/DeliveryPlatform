using System;
using Identity.Contract;
using Shared.Interfaces;

namespace Shared
{
    public class ExecutionContext : IExecutionContext
    {
        private readonly object _padlock = new object();

        public Role UserRole { get; private set; }

        public string UserId { get; private set; }
        
        public bool IsInitialized { get; private set; }

        public void Initialize(Role userRole, string userId)
        {
            lock (_padlock)
            {
                if (string.IsNullOrEmpty(userId))
                {
                    throw new ArgumentNullException(nameof(userId));
                }
                if (IsInitialized)
                {
                    throw new Exception("Cannot initialize twice");
                }

                IsInitialized = true;
                UserRole = userRole;
                UserId = userId;
            }
        }
    }
}
