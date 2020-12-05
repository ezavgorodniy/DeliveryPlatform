using System;
using Identity.DataLayer.Interfaces;
using Identity.DataLayer.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Identity.DataLayer
{
    public static class Configuration
    {
        public static void ConfigureServices(IServiceCollection serviceCollection)
        {
            if (serviceCollection == null)
            {
                throw new ArgumentNullException(nameof(serviceCollection));
            }

            serviceCollection.AddTransient<IUsersRepo, UsersRepo>();
        }
    }
}
