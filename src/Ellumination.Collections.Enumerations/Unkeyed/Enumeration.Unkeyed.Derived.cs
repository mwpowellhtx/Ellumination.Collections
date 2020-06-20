using System.Collections.Generic;
using System.Linq;

namespace Ellumination.Collections.Unkeyed
{
    /// <summary>
    /// Sometimes a strongly typed <see cref="Enumeration"/> may be required apart from
    /// <see cref="Keyed.Enumeration{TKey,T}"/> Key details. Keyed
    /// <see cref="Keyed.Enumeration{TKey,T}"/> are the natural course of utilizing the
    /// Enumerations framework. However, <see cref="Enumeration{T}"/> are the exception
    /// to the rule. So we present these in their own namespace.
    /// </summary>
    /// <typeparam name="T">Represents the Derived Enumeration Type.</typeparam>
    public abstract class Enumeration<T> : Enumeration
        where T : Enumeration<T>
    {
        /// <summary>
        /// Protected Default Constructor.
        /// </summary>
        protected Enumeration()
        {
        }

        /// <summary>
        /// Protected Constructor.
        /// </summary>
        /// <param name="name"></param>
        protected Enumeration(string name)
            : base(name)
        {
        }

        /// <summary>
        /// Protected Constructor.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="displayName"></param>
        protected Enumeration(string name, string displayName)
            : base(name, displayName)
        {
        }

        /// <inheritdoc />
        protected override string ResolveName()
        {
            // TODO: TBD: may also want to restrict the field type(s) to those of the declaring type
            var field = GetDeclaringTypes(typeof(T))
                .SelectMany(type => type.GetFields(PublicStaticDeclaredOnly))
                .FirstOrDefault(fieldInfo => ReferenceEquals(this, fieldInfo.GetValue(null)));

            return field?.Name ?? string.Empty;
        }

        /// <summary>
        /// <see cref="Values"/> backing field.
        /// </summary>
        private static IEnumerable<T> _values;

        /// <summary>
        /// Gets the enumerated Values from the <typeparamref name="T"/> type.
        /// </summary>
        /// <see cref="Enumeration.GetValues{T}"/>
        public static IEnumerable<T> Values => _values ?? (_values = GetValues<T>().ToArray());
    }
}
