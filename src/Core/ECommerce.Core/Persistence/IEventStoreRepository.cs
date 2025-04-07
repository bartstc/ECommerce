using Ecommerce.Core.Domain;
using Marten.Events;

namespace ECommerce.Core.Persistence;

public interface IEventStoreRepository<TA>
    where TA : class, IAggregateRoot<StronglyTypedId<Guid>>
{
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
    long AppendEvents(TA aggregate);
    Task<IEventStream<A>> FetchForWriting<A>(Guid id, CancellationToken cancellationToken = default)
        where A : class, IAggregateRoot<StronglyTypedId<Guid>>;
    void StoreDocument<TDocument>(params TDocument[] entities);
    // void AppendToOutbox(INotification @event);
}