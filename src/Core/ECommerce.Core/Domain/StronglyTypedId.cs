﻿using ECommerce.Core.Exceptions;

namespace Ecommerce.Core.Domain;

public abstract class StronglyTypedId<T> : ValueObject<StronglyTypedId<T>>
{
	public T Value { get; }

	public StronglyTypedId(T value)
	{
		if (value is null)
			throw new ArgumentNullException(nameof(value));
		if (value.Equals(Guid.Empty))
			throw new BusinessValidationException("A valid id must be provided.");

		Value = value;
	}

	public override int GetHashCode()
		=> EqualityComparer<T>.Default.GetHashCode(Value);

	public static bool operator ==(StronglyTypedId<T>? left, StronglyTypedId<T>? right)
		=> Equals(left, right);

	public static bool operator !=(StronglyTypedId<T>? left, StronglyTypedId<T>? right)
		=> !Equals(left, right);

	protected override IEnumerable<object> GetEqualityComponents()
	{
		yield return Value;
	}
}