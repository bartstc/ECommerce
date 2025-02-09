namespace Domain
{
    public interface IStoreRepository
    {
        Task<Store> GetStore(Guid id);
        void CreateStore(Store store);
        void UpdateStore(Store store);
        void DeleteStore(Store store);
        Task<bool> Complete();
    }
}