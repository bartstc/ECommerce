using Marten.Events;
using Microsoft.Extensions.Logging;

namespace Ecommerce.Core.Infrastructure.EventStore;

public class EventStoreRepository<TA>(
    IDocumentSession documentSession,
    ILogger<EventStoreRepository<TA>> logger
) : IEventStoreRepository<TA> where TA : class, IAggregateRoot<StronglyTypedId<Guid>>
{
    private readonly IDocumentSession _documentSession = documentSession
                                                         ?? throw new ArgumentNullException(nameof(documentSession));

    private readonly ILogger<EventStoreRepository<TA>> _logger = logger
                                                                 ?? throw new ArgumentNullException(nameof(logger));

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _documentSession.SaveChangesAsync(cancellationToken);
    }

    // Stores uncommited events from an aggregate 
    public long AppendEvents(TA aggregate)
    {
        var events = aggregate.GetUncommittedEvents().ToArray();
        var nextVersion = aggregate.Version + events.Length;

        aggregate.ClearUncommittedEvents();
        _documentSession.Events.Append(aggregate.Id.Value, nextVersion, events);

        return nextVersion;
    }

    public async Task<IEventStream<TA>> FetchForWriting<TA>(Guid id, CancellationToken cancellationToken = default)
        where TA : class, IAggregateRoot<StronglyTypedId<Guid>>
    {
        var aggregate = await _documentSession.Events.FetchForWriting<TA>(id, cancellationToken);
        return aggregate;
    }
    
    public async Task<TP> FetchLatest<TP>(Guid id, CancellationToken cancellationToken = default)
        where TP : class
    {
        var aggregate = await _documentSession.Events.FetchLatest<TP>(id, cancellationToken);
        return aggregate;
    }

    public void StoreDocument<TD>(params TD[] documents)
    {
        _documentSession.Store(documents);
    }
}