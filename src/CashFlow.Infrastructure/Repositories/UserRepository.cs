using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories.User;
using CashFlow.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace CashFlow.Infrastructure.Repositories;

public class UserRepository : IUserReadOnlyRepository, IUserWriteOnlyRepository
{
    private readonly CashFlowDbContext _context;

    public UserRepository(CashFlowDbContext context)
    {
        _context = context;
    }

    public async Task<bool> ExistActiveUserWithEmail(string email)
    {
        return await _context.Users.AnyAsync(a=>a.Email.Equals(email));
    }

    public async Task<User?> GetUserByEmail(string email)
    {
        return await _context.Users.FirstOrDefaultAsync(a => a.Email.Equals(email));
    }

    public async Task AddAsync(User user)
    {
        await _context.Users.AddAsync(user);
    }
}