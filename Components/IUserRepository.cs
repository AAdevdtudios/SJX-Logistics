using SjxLogistics.Models.DatabaseModels;
using System.Threading.Tasks;

namespace SjxLogistics.Components
{
    public interface IUserRepository
    {
        Task<Users> GetByEmail(string email);
        Task<Users> CreateUser(Users users);
    }
}
