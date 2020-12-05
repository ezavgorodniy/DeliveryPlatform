using Microsoft.Extensions.DependencyInjection;
using System;
using Shared.Helpers;
using Shared.Interfaces;

namespace Shared
{
    public static class Configuration
    {
        public static void ConfigureServices(IServiceCollection serviceCollection)
        {
            if (serviceCollection == null)
            {
                throw new ArgumentNullException(nameof(serviceCollection));
            }

            serviceCollection.AddTransient<IExecutionContextHelper, ExecutionContextHelper>();
        }
    }
}
