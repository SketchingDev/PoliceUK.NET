namespace PoliceUk.Tests.Unit.CustomAssertions.Equality
{
    using System;
    using System.Collections.Generic;

    public abstract class AbstractEqualityComparer<T> : IEqualityComparer<T>
    {
        public abstract bool AreEqual(T x, T y);

        public bool Equals(T x, T y)
        {
            if (x == null && y == null) return true;
            if (x == null || y == null) return false;

            return this.AreEqual(x, y);
        }

        public int GetHashCode(T obj)
        {
            throw new NotImplementedException();
        }
    }
}
