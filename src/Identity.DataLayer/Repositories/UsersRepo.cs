using Identity.DataLayer.DataModels;
using Identity.DataLayer.Interfaces;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Identity.Contract;

namespace Identity.DataLayer.Repositories
{
    internal class UsersRepo : IUsersRepo
    {
        private readonly List<User> _users = new List<User>
        {
            new User
            {
                Id = "d9ac3e47-e5a3-4219-ae65-d08cef30e05a",
                UserName = "TheUser",
                Password = "TheUser",
                Role = Role.User
            },
            new User
            {
                Id = "ae05a857-21f6-4c53-80cd-3a891165813a",
                UserName = "ThePartner",
                Password = "ThePartner",
                Role = Role.Partner
            }
        };

        public Task<User> Authenticate(string username, string password)
        {
            var result = _users.FirstOrDefault(user => user.UserName == username && user.Password == password);
            return Task.FromResult(result);
        }
    }
}
