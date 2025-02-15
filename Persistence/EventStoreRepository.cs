using Ecommerce.Core.Domain;
using ECommerce.Core.Persistence;
using Marten;
using Microsoft.Extensions.Logging;

namespace Persistence;

public class EventStoreRepository<TA>(
    IDocumentSession documentSession,
    ILogger<EventStoreRepository<TA>> logger
) : IEventStoreRepository<TA> where TA : class, IAggregateRoot<StronglyTypedId<Guid>>
{
    private readonly IDocumentSession _documentSession = documentSession
        ?? throw new ArgumentNullException(nameof(documentSession));
    private readonly ILogger<EventStoreRepository<TA>> _logger = logger
        ?? throw new ArgumentNullException(nameof(logger));

    /// <summary>
    /// Stores uncommited events from an aggregate 
    /// </summary>
    /// <param name="aggregate"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<long> AppendEventsAsync(TA aggregate, CancellationToken cancellationToken = default)
    {
        var events = aggregate.GetUncommittedEvents().ToArray();
        var nextVersion = aggregate.Version + events.Length;

        aggregate.ClearUncommittedEvents();
        _documentSession.Events.Append(aggregate.Id.Value, nextVersion, events);

        await _documentSession.SaveChangesAsync();

        return nextVersion;
    }

    /// <summary>
    /// Fetch domain events from the stream
    /// </summary>
    /// <param name="id"></param>
    /// <param name="version"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public async Task<TA> FetchStreamAsync(Guid id, int? version = null, CancellationToken cancellationToken = default)
    {
        var aggregate = await _documentSession.Events.AggregateStreamAsync<TA>(id, version ?? 0);
        return aggregate ?? throw new InvalidOperationException($"No aggregate found with id {id}.");
    }
}