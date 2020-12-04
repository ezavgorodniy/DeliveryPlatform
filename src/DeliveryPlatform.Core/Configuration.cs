using System;
using DeliveryPlatform.Core.Interfaces;
using DeliveryPlatform.Core.Mappers;
using DeliveryPlatform.Core.Services;
using Microsoft.Extensions.DependencyInjection;

namespace DeliveryPlatform.Core
{
    public static class Configuration
    {
        public static void ConfigureServices(IServiceCollection serviceCollection)
        {
            if (serviceCollection == null)
            {
                throw new ArgumentNullException(nameof(serviceCollection));
            }

            serviceCollection.AddTransient<IDeliveryMapper, DeliveryMapper>();
            serviceCollection.AddTransient<IDeliveryService, DeliveryService>();
        }
    }
}
