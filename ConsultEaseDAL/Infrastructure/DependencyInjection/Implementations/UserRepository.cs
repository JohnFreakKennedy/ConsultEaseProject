using System.Collections.Generic;
using System.Threading.Tasks;
using ConsultEaseDAL.Context;
using ConsultEaseDAL.Entities.Auth;
using ConsultEaseDAL.Infrastructure.DependencyInjection.Abstractions;
using ConsultEaseDAL.Infrastructure.Implementations.Base;
using Microsoft.EntityFrameworkCore;

namespace ConsultEaseDAL.Infrastructure.DependencyInjection.Implementations;

public class UserRepository(ConsultEaseDbContext dbContext) 
    : RepositoryBase<int, User>(dbContext), IUserRepository
{
    public async Task<IEnumerable<User>> GetAllUsersAsync() =>
        await dbContext.Users.ToListAsync();

    public async Task<User?> GetUserByIdAsync(int id) =>
        await FindByKeyAsync(id);

    public async Task<User?> CreateUserAsync(User user) =>
        await CreateAsync(user);

    public async Task<int> UpdateUserAsync(User user) =>
        await UpdateAsync(user);

    public async Task<int> DeleteUserAsync(User user) =>
        await DeleteAsync(user);
}