using System;

namespace Ellumination.Collections.Variants
{
    public class BaseClass : IEquatable<BaseClass>, IComparable<BaseClass>
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        private bool CanCompare(BaseClass other) => other != null && GetType() == other.GetType();

        public bool Equals(BaseClass other) => CanCompare(other) && Id.Equals(other.Id);

        public int CompareTo(BaseClass other) => CanCompare(other) ? Id.CompareTo(other.Id) : -1;
    }
}
