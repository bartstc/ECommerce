using Marten;
using Marten.Events;
using Microsoft.Extensions.Logging;

namespace ProductCatalog.Infrastructure;

public class EventStoreRepository<TA>(
    IDocumentSession documentSession,
    ILogger<EventStoreRepository<TA>> logger
) : IEventStoreRepository<TA> where TA : class, IAggregateRoot<StronglyTypedId<Guid>>
{
    private readonly IDocumentSession _documentSession = documentSession
        ?? throw new ArgumentNullException(nameof(documentSession));
    private readonly ILogger<EventStoreRepository<TA>> _logger = logger
        ?? throw new ArgumentNullException(nameof(logger));

    // Stores uncommited events from an aggregate 
    public async Task<long> AppendEventsAsync(TA aggregate, CancellationToken cancellationToken = default)
    {
        var events = aggregate.GetUncommittedEvents().ToArray();
        var nextVersion = aggregate.Version + events.Length;

        aggregate.ClearUncommittedEvents();
        _documentSession.Events.Append(aggregate.Id.Value, nextVersion, events);

        await _documentSession.SaveChangesAsync();

        return nextVersion;
    }

    [Obsolete("FetchStreamAsync is obsolete. Use FetchForWriting instead.")]
    public async Task<TA> FetchStreamAsync(Guid id, int? version = null, CancellationToken cancellationToken = default)
    {
        var aggregate = await _documentSession.Events.AggregateStreamAsync<TA>(id, version ?? 0);
        return aggregate ?? null;
    }

    public async Task<IEventStream<A>> FetchForWriting<A>(Guid id, CancellationToken cancellationToken = default) where A : class, IAggregateRoot<StronglyTypedId<Guid>>
    {
        var aggregate = await _documentSession.Events.FetchForWriting<A>(id, cancellationToken);
        return aggregate;
    }
}