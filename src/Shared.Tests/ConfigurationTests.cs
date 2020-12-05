using System;
using Microsoft.Extensions.DependencyInjection;
using Shared.Helpers;
using Shared.Interfaces;
using Xunit;

namespace Shared.Tests
{
    public class ConfigurationTests
    {
        protected readonly ServiceProvider _serviceProvider;

        public ConfigurationTests()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddOptions();
            Configuration.ConfigureServices(serviceCollection);
            
            _serviceProvider = serviceCollection.BuildServiceProvider();
        }

        [Fact]
        public void ConfigureServicesNullServiceCollectionExpectArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => Configuration.ConfigureServices(null));
        }

        [Fact]
        public void ExpectUsersRepoToBeResolvable()
        {
            var authenticationService = _serviceProvider.GetRequiredService<IExecutionContextHelper>();

            Assert.IsType<ExecutionContextHelper>(authenticationService);
        }
    }
}
