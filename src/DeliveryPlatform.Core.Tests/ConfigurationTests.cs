using Microsoft.Extensions.DependencyInjection;
using System;
using DeliveryPlatform.Core.Interfaces;
using DeliveryPlatform.Core.Services;
using Xunit;

namespace DeliveryPlatform.Core.Tests
{
    public class ConfigurationTests
    {
        protected readonly ServiceProvider _serviceProvider;

        public ConfigurationTests()
        {
            var serviceCollection = new ServiceCollection();
            // datalayer and options are prerequisite
            serviceCollection.AddLogging();
            serviceCollection.AddOptions();
            DataLayer.Configuration.ConfigureServices(serviceCollection);
            Configuration.ConfigureServices(serviceCollection);
            _serviceProvider = serviceCollection.BuildServiceProvider();
        }

        [Fact]
        public void ConfigureServicesNullServiceCollectionExpectArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => Configuration.ConfigureServices(null));
        }

        [Fact]
        public void ExpectDeliveryServiceToBeResolvable()
        {
            var deliveryService = _serviceProvider.GetRequiredService<IDeliveryService>();

            Assert.IsType<DeliveryService>(deliveryService);
        }

        [Fact]
        public void ExpectExpirationServiceToBeResolvable()
        {
            var expirationService = _serviceProvider.GetRequiredService<IExpirationService>();

            Assert.IsType<ExpirationService>(expirationService);
        }
    }
}
