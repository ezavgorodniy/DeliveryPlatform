using System;
using Identity.Core.Interfaces;
using Identity.Core.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Identity.Core
{
    public static class Configuration
    {
        public static void ConfigureServices(IServiceCollection serviceCollection)
        {
            if (serviceCollection == null)
            {
                throw new ArgumentNullException(nameof(serviceCollection));
            }

            serviceCollection.AddTransient<IAuthenticationService, AuthenticationService>();
        }
    }
}
