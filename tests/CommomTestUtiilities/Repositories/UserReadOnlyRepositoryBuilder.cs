using CashFlow.Domain.Repositories.User;
using Moq;

namespace CommomTestUtiilities.Repositories;

public class UserReadOnlyRepositoryBuilder
{
    private readonly Mock<IUserReadOnlyRepository> _repository;

    public UserReadOnlyRepositoryBuilder()
    {
        _repository = new Mock<IUserReadOnlyRepository>();
    }
    
    public IUserReadOnlyRepository Buiild() => _repository.Object;
} 