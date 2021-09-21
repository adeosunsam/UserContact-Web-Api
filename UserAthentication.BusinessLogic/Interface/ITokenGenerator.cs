using System.Threading.Tasks;
using UserAthentication.Model;

namespace UserAthentication
{
    public interface ITokenGenerator
    {
        Task<string> GetToken(Users user);
        
    }
}