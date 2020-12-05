using System.Threading.Tasks;
using Identity.Core.Models;

namespace Identity.Core.Interfaces
{
    public interface IAuthenticationService
    {
        Task<AuthenticateResponse> Authenticate(AuthenticateRequestDto authenticationRequestDto);
    }
}
