using System;
using Identity.Contract;
using Shared.Interfaces;

namespace Shared
{
    public class ExecutionContext : IExecutionContext
    {
        private readonly object _padlock = new object();

        private bool _isInitialized = false;

        public Role UserRole { get; private set; }

        public string UserId { get; private set; }

        public void Initialize(Role userRole, string userId)
        {
            lock (_padlock)
            {
                if (string.IsNullOrEmpty(userId))
                {
                    throw new ArgumentNullException(nameof(userId));
                }
                if (_isInitialized)
                {
                    throw new Exception("Cannot initialize twice");
                }

                _isInitialized = true;
                UserRole = userRole;
                UserId = userId;
            }
        }
    }
}
