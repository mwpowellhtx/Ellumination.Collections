using System;

namespace Ellumination.Collections.Variants
{
    public class DerivedClass : BaseClass, IEquatable<DerivedClass>, IComparable<DerivedClass>
    {
        public bool Equals(DerivedClass other) => base.Equals(other);

        public int CompareTo(DerivedClass other) => base.CompareTo(other);
    }
}
