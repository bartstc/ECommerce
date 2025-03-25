using Marten.Events.Daemon.Resiliency;
using Newtonsoft.Json;
using Weasel.Core;

namespace Ecommerce.Core.Infrastructure.EventStore;

public static class MartenConfigExtension
{
    public static void AddMarten(this IServiceCollection services,
        IConfiguration configuration,
        Action<StoreOptions>? configureOptions = null)
    {
        if (services is null)
            throw new ArgumentNullException(nameof(services));

        var connectionString = configuration.GetConnectionString("DefaultConnection");
        var martenConfig = configuration.GetSection("EventStore").Get<MartenSettings>();

        if (string.IsNullOrEmpty(connectionString))
            throw new ArgumentNullException("EventStore connection string is missing");

        if (string.IsNullOrEmpty(martenConfig?.WriteSchema))
            throw new ArgumentNullException("EventStore writeSchema is missing");

        services.AddMarten(options =>
        {
            options.Connection(connectionString);
            options.AutoCreateSchemaObjects = AutoCreate.All;
            options.UseNewtonsoftForSerialization(nonPublicMembersStorage: NonPublicMembersStorage.All);

            options.Events.DatabaseSchemaName = martenConfig.WriteSchema;

            if (!string.IsNullOrEmpty(martenConfig.ReadSchema))
                options.DatabaseSchemaName = martenConfig.ReadSchema;

            // outbox
            options.Schema.For<IntegrationEvent>()
                .DatabaseSchemaName("public")
                .DocumentAlias("outboxmessages");

            // Custom store options
            configureOptions?.Invoke(options);
        })
        .AddAsyncDaemon(DaemonMode.HotCold)
        .UseLightweightSessions();

        // Wrapper for IQuerySession 
        services.AddScoped<IQuerySessionWrapper, QuerySessionWrapper>();
    }
}

public class MartenSettings
{
    public string WriteSchema { get; set; }
    public string ReadSchema { get; set; }
}

public interface IIntegrationEvent : INotification
{
    public Guid Id { get; }
}

public class IntegrationEvent : IIntegrationEvent
{
    public Guid Id { get; } = Guid.NewGuid();
    public string EventName { get; } // Event name identifier
    public string JSON_Payload { get; } // Serialized data

    public static IntegrationEvent FromNotification(INotification domainEvent)
    {
        if (domainEvent == null)
            throw new ArgumentNullException(nameof(domainEvent));

        return new IntegrationEvent(domainEvent);
    }

    public IntegrationEvent() { }

    private IntegrationEvent(INotification @event)
    {
        EventName = @event.GetType().Name;
        JSON_Payload = JsonConvert.SerializeObject(@event);
    }
}

// Wrapper of IQueryable, allowing it to be easily mockable
public interface IQuerySessionWrapper
{
    IQueryable<T> Query<T>();
}

public class QuerySessionWrapper : IQuerySessionWrapper
{
    private readonly IQuerySession _querySession;

    public QuerySessionWrapper(IQuerySession querySession)
    {
        _querySession = querySession;
    }

    public IQueryable<T> Query<T>()
    {
        return _querySession.Query<T>();
    }
}
