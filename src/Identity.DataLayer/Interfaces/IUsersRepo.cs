using System.Threading.Tasks;
using Identity.DataLayer.DataModels;

namespace Identity.DataLayer.Interfaces
{
    public interface IUsersRepo
    {

        /// <summary>
        /// return user based on username and password
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="password">Password</param>
        /// <returns></returns>
        Task<User> Authenticate(string username, string password);
    }
}
