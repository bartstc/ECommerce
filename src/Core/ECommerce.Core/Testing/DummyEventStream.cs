using Ecommerce.Core.Domain;
using Marten.Events;

namespace ECommerce.Core.Testing;

public class DummyEventStream<A> : IEventStream<A>
    where A : class, IAggregateRoot<StronglyTypedId<Guid>>
{
    public Guid Id { get; }
    public long Version { get; }
    public IReadOnlyList<Marten.Events.IEvent> Events { get; }
    public A Aggregate { get; private set; }
    public CancellationToken Cancellation { get; }
    public long? CurrentVersion { get; private set; }
    public string Key => Id.ToString();
    public long? StartingVersion { get; private set; }

    public DummyEventStream(Guid id, long version, IReadOnlyList<Marten.Events.IEvent> events, A aggregate = null,
        CancellationToken cancellation = default)
    {
        Id = id;
        Version = version;
        Events = events;
        Aggregate = aggregate;
        Cancellation = cancellation;
        CurrentVersion = version;
        StartingVersion = version;
    }

    public void AppendMany(params object[] events)
    {
    }

    public void AppendMany(IEnumerable<object> events)
    {
    }

    public void AppendOne(object @event)
    {
    }
}