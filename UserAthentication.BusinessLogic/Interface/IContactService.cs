using System.Threading.Tasks;
using UserAthemtication.DTOs;
using UserAthentication.Common;
using UserAthentication.Model;

namespace UserAthentication.BusinessLogic
{
    public interface IContactService
    {
        Task<bool> DeleteUserById(string userId);
        Task<UserResponse> GetUserById(string userId);
        Task<UserResponse> GetUserByEmail(string email);
        Task<bool> UpdateUser(string userId, UpdateRequest updateRequest);
        PagedList<Users> GetAll(Pagination pagination);
    }
}