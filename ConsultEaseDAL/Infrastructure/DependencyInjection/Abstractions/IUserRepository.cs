using System.Collections.Generic;
using System.Threading.Tasks;
using ConsultEaseDAL.Entities.Auth;

namespace ConsultEaseDAL.Infrastructure.DependencyInjection.Abstractions;

public interface IUserRepository
{
    Task<IEnumerable<User>> GetAllUsersAsync();
    Task<User?> GetUserByIdAsync(int id);
    Task<User?> CreateUserAsync(User user);
    Task<int> UpdateUserAsync(User user);
    Task<int> DeleteUserAsync(User user);
}