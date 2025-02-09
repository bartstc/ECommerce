namespace Domain
{
    public interface IStoreRepository
    {
        Task<Store> GetStore(Guid id);
        Task<bool> CreateStore(Store store);
        Task<bool> UpdateStore(Store store);
        Task<bool> DeleteStore(Store store);
    }
}