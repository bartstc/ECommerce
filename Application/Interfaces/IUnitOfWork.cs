namespace Application.Interfaces
{
    public interface IUnitOfWork
    {
        Task<bool> Complete();
    }
}