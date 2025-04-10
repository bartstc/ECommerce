using Ecommerce.Core.Domain;
using ECommerce.Core.Persistence;
using JasperFx.Core;
using Marten.Events;

namespace ECommerce.Core.Testing;

public class DummyEventStoreRepository<TA> : IEventStoreRepository<TA>
    where TA : class, IAggregateRoot<StronglyTypedId<Guid>>
{
    private readonly List<DummyEventStream<TA>> eventStreams = new();
    private readonly List<StreamAction> streamActions = new();

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }

    public long AppendEvents(TA aggregate)
    {
        var nextVersion = aggregate.Version + 1;
        var events = aggregate.GetUncommittedEvents().ToList();

        var stream = eventStreams.FirstOrDefault(s => s.Id == aggregate.Id.Value);
        if (stream == null)
        {
            stream = new DummyEventStream<TA>(aggregate.Id.Value, 0, new List<IEvent>(), aggregate);
            eventStreams.Add(stream);
        }

        stream.AppendMany(events);

        streamActions.Add(new StreamAction(
            aggregate.Id.Value,
            aggregate,
            nextVersion,
            events
        ));

        return nextVersion;
    }

    public Task<IEventStream<A>> FetchForWriting<A>(Guid id, CancellationToken cancellationToken = default)
        where A : class, IAggregateRoot<StronglyTypedId<Guid>>
    {
        var stream = eventStreams.FirstOrDefault(s => s.Id == id) as DummyEventStream<A>;
        if (stream == null)
        {
            stream = new DummyEventStream<A>(
                id,
                0,
                new List<IEvent>(),
                null,
                cancellationToken
            );
            eventStreams.Add(stream as DummyEventStream<TA>);
        }

        return Task.FromResult<IEventStream<A>>(stream);
    }

    public async Task<TP> FetchLatest<TP>(Guid id, CancellationToken cancellationToken = default)
        where TP : class
    {
       throw new NotImplementedException();
    }

    public void StoreDocument<TDocument>(params TDocument[] documents)
    {
    }

    public record StreamAction(
        Guid Stream,
        TA Aggregate,
        long ExpectedVersion,
        IEnumerable<object> Events
    );
}