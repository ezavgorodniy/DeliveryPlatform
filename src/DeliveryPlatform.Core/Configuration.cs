using System;
using DeliveryPlatform.Core.Helpers;
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
            serviceCollection.AddTransient<IOrderMapper, OrderMapper>();
            serviceCollection.AddTransient<IRecipientMapper, RecipientMapper>();
            serviceCollection.AddTransient<IAccessWindowMapper, AccessWindowMapper>();

            serviceCollection.AddTransient<IPermissionChecker, PermissionChecker>();

            serviceCollection.AddTransient<IDeliveryService, DeliveryService>();
        }
    }
}
