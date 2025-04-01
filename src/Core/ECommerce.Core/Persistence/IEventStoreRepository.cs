using Ecommerce.Core.Domain;
using Marten.Events;

namespace ECommerce.Core.Persistence;

public interface IEventStoreRepository<TA>
    where TA : class, IAggregateRoot<StronglyTypedId<Guid>>
{
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
    long AppendEventsAsync(TA aggregate, CancellationToken cancellationToken = default);
    Task<TA> FetchStreamAsync(Guid id, int? version = null, CancellationToken cancellationToken = default);
    Task<IEventStream<A>> FetchForWriting<A>(Guid id, CancellationToken cancellationToken = default) where A : class, IAggregateRoot<StronglyTypedId<Guid>>;
    void StoreDocument<TDocument>(TDocument document, CancellationToken cancellationToken = default);
    // void AppendToOutbox(INotification @event);
}