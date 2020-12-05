using Microsoft.Extensions.DependencyInjection;
using System;
using Identity.DataLayer.Interfaces;
using Identity.DataLayer.Repositories;
using Xunit;

namespace Identity.DataLayer.Tests
{
    public class ConfigurationTests
    {
        protected readonly ServiceProvider _serviceProvider;

        public ConfigurationTests()
        {
            var serviceCollection = new ServiceCollection();
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
            var usersRepo = _serviceProvider.GetRequiredService<IUsersRepo>();

            Assert.IsType<UsersRepo>(usersRepo);
        }
    }
}
