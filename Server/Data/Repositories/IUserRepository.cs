using Server.Models;
using System.Threading.Tasks;

namespace Server.Data.Repositories;

public interface IUserRepository
{
    Task<User?> GetByUsernameAsync(string username);
    Task<int> AddAsync(User user);
}