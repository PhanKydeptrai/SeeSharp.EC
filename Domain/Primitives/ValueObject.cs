﻿namespace Domain.Primitives;

public abstract class ValueObject : IEquatable<ValueObject>
{
    public abstract IEnumerable<object> GetAtomicValues();

    public bool Equals(ValueObject? other)
    {
        return other is not null && ValuesAreEqual(other);
    }

    public override bool Equals(object? obj)
    {
        return obj is ValueObject valueObject && ValuesAreEqual(valueObject);
    }

    public override int GetHashCode()
    {
        return GetAtomicValues()
            .Aggregate(default(int),
            HashCode.Combine);
    }

    private bool ValuesAreEqual(ValueObject valueObject)
    {
        return GetAtomicValues().SequenceEqual(valueObject.GetAtomicValues());
    }
}
