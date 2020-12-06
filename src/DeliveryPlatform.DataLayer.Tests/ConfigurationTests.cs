using System;
using DeliveryPlatform.DataLayer.Interfaces;
using DeliveryPlatform.DataLayer.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace DeliveryPlatform.DataLayer.Tests
{
    public class ConfigurationTests
    {
        protected readonly ServiceProvider _serviceProvider;

        public ConfigurationTests()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddLogging();
            Configuration.ConfigureServices(serviceCollection);
            _serviceProvider = serviceCollection.BuildServiceProvider();
        }

        [Fact]
        public void ConfigureServicesNullServiceCollectionExpectArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => Configuration.ConfigureServices(null));
        }

        [Fact]
        public void ExpectDeliveryRepoToBeResolvable()
        {
            var deliveryService = _serviceProvider.GetRequiredService<IDeliveryRepository>();

            Assert.IsType<DeliveryRepository>(deliveryService);
        }
    }
}
