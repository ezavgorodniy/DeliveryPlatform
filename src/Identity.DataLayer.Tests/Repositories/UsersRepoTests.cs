using System.Threading.Tasks;
using Identity.Contract;
using Identity.DataLayer.Repositories;
using Xunit;

namespace Identity.DataLayer.Tests.Repositories
{
    public class  UsersRepoTests
    {
        private readonly UsersRepo _usersRepo;

        public UsersRepoTests()
        {
            _usersRepo = new UsersRepo();
        }

        [Theory]
        [InlineData("TheUser", "TheUser", Role.User)]
        [InlineData("ThePartner", "ThePartner", Role.Partner)]
        public async Task AuthenticateCorrectUserAndPasswordExpectCorrectRole(string user, string password, Role role)
        {
            var resultUser = await _usersRepo.Authenticate(user, password);

            Assert.NotNull(resultUser);
            Assert.Equal(role, resultUser.Role);
        }

        [Theory]
        [InlineData("TheUser", "ThePartner")]
        [InlineData("ThePartner", "TheUser")]
        [InlineData("", "TheUser")]
        [InlineData("TheUser", "")]
        [InlineData("TheUser", null)]
        [InlineData(null, "ThePartner")]
        public async Task AuthenticateUnknownUserExpecNull(string user, string password)
        {
            var resultUser = await _usersRepo.Authenticate(user, password);

            Assert.Null(resultUser);
        }
    }
}
