using System.Text.Json.Serialization;
using ECommerce.Core.Domain;
using ECommerce.Core.Exceptions;
using Marten.Schema;

namespace Ecommerce.Core.Domain;

public abstract class AggregateRoot<TKey> : Entity<TKey>, IAggregateRoot<TKey>
	where TKey : StronglyTypedId<Guid>
{
	[Identity]
	public Guid AggregateId
	{
		get => Id.Value;
		set { }
	}

	public long Version { get; protected set; }

	public IEnumerable<IDomainEvent> GetUncommittedEvents()
		=> _uncommittedEvents;

	public void ClearUncommittedEvents()
		=> _uncommittedEvents.Clear();

	protected void AppendEvent(IDomainEvent @event)
		=> _uncommittedEvents.Enqueue(@event);

	protected void CheckRule(IBusinessRule rule)
	{
		if (rule.IsBroken())
			throw new BusinessRuleException(rule);
	}

	protected static async Task CheckRuleAsync(IBusinessRuleAsync rule)
	{
		if (await rule.IsBrokenAsync())
			throw new BusinessRuleException(rule);
	}

	[JsonIgnore]
	private readonly Queue<IDomainEvent> _uncommittedEvents = new Queue<IDomainEvent>();
}

//https://event-driven.io/en/using_strongly_typed_ids_with_marten/