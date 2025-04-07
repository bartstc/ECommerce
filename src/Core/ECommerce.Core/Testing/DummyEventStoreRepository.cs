using Ecommerce.Core.Domain;
using ECommerce.Core.Persistence;

namespace ECommerce.Core.Testing;

public class DummyEventStoreRepository<TA> : IEventStoreRepository<TA>
    where TA : class, IAggregateRoot<StronglyTypedId<Guid>>
{
    public List<StreamAction> AggregateStream = new();

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }

    public long AppendEvents(TA aggregate)
    {
        var nextVersion = aggregate.Version + 1;
        AggregateStream.Add(new StreamAction(
            aggregate.Id.Value,
            aggregate, nextVersion,
            aggregate.GetUncommittedEvents())
        );

        return nextVersion;
    }

    public Task<Marten.Events.IEventStream<A>> FetchForWriting<A>(Guid id,
        CancellationToken cancellationToken = default)
        where A : class, IAggregateRoot<StronglyTypedId<Guid>>
    {
        var streamAction = AggregateStream.FirstOrDefault(c => c.Stream == id);
        var eventStream = new DummyEventStream<A>(
            id,
            streamAction?.ExpectedVersion ?? 0,
            new List<Marten.Events.IEvent>(),
            streamAction?.Aggregate as A,
            cancellationToken
        );

        return Task.FromResult<Marten.Events.IEventStream<A>>(eventStream);
    }

    public void StoreDocument<TDocument>(params TDocument[] documents)
    {
    }

    public record class StreamAction(
        Guid Stream,
        TA Aggregate,
        long ExpectedVersion,
        IEnumerable<object> Events
    );
}