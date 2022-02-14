using SjxLogistics.Data;
using SjxLogistics.Models.DatabaseModels;
using System.Linq;
using System.Threading.Tasks;

namespace SjxLogistics.Components
{
    public class AuthenticationRepository : IUserRepository
    {
        public readonly DataBaseContext _context;
        public Task<Users> CreateUser(Users users)
        {
            _context.Users.Add(users);
            return Task.FromResult(users);
        }

        public Task<Users> GetByEmail(string email)
        {
            return Task.FromResult(_context.Users.FirstOrDefault(i => i.Email == email));
        }
    }
}
