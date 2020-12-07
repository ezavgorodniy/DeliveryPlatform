using System;
using DeliveryPlatform.DataLayer.Interfaces;
using DeliveryPlatform.DataLayer.Mongo;
using DeliveryPlatform.DataLayer.Mongo.Interfaces;
using DeliveryPlatform.DataLayer.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace DeliveryPlatform.DataLayer
{
    public static class Configuration
    {
        public static void ConfigureServices(IServiceCollection serviceCollection)
        {
            if (serviceCollection == null)
            {
                throw new ArgumentNullException(nameof(serviceCollection));
            }

            serviceCollection.AddSingleton<IMongoDatabaseFactory, MongoDatabaseFactory>();

            serviceCollection.AddTransient<IDeliveryRepository, DeliveryRepository>();
        }
    }
}
