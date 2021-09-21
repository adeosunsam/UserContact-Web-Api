using System.Threading.Tasks;
using UserAthemtication.DTOs;

namespace UserAthentication.BusinessLogic
{
    public interface IAuthentication
    {
        Task<UserResponse> Login(UserLogin userLogin);
        Task<UserResponse> Register(string role, RegistrationRequest registrationRequest);
    }
}