namespace CashFlow.Domain.Repositories.User;

public interface IUserWriteOnlyRepository
{
    Task AddAsync(Entities.User user);
}