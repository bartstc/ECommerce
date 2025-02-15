using Ecommerce.Core.Domain;

namespace ECommerce.Core.Persistence;

public interface IEventStoreRepository<TA>
    where TA : class, IAggregateRoot<StronglyTypedId<Guid>>
{
    Task<long> AppendEventsAsync(TA aggregate, CancellationToken cancellationToken = default);
    Task<TA> FetchStreamAsync(Guid id, int? version = null, CancellationToken cancellationToken = default);
    // void AppendToOutbox(INotification @event);
}