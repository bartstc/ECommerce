namespace ECommerce.Core.Persistence;

public interface IUnitOfWork
{
    Task<bool> Complete();
}