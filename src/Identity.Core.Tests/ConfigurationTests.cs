using System;
using Identity.Core.Interfaces;
using Identity.Core.Services;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Identity.Core.Tests
{
    public class ConfigurationTests
    {
        protected readonly ServiceProvider _serviceProvider;

        public ConfigurationTests()
        {
            var serviceCollection = new ServiceCollection();
            // datalayer and options are prerequisite
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
        public void ExpectAuthenticationServiceToBeResolvable()
        {
            var authenticationService = _serviceProvider.GetRequiredService<IAuthenticationService>();

            Assert.IsType<AuthenticationService>(authenticationService);
        }
    }
}
