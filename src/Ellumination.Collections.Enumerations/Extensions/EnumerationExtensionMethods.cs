using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Ellumination.Collections
{
    /// <summary>
    /// Extension methods provided for purposes of supporting Enumeration behavior.
    /// </summary>
    public static class EnumerationExtensionMethods
    {
        // TODO: TBD: include this one with the attribute...
        /// <summary>
        /// Returns the <see cref="Type"/> fixtures that are marked to declare
        /// <see cref="Enumeration"/> values.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        internal static IEnumerable<Type> GetEnumeratedValueTypes(this Type type)
        {
            var returned = false;

            var enumerationTypes = new[] {typeof(Enumeration)};

            // It is far easier to reason about this one than to recurse.
            for (var current = type;
                current != null
                && !enumerationTypes.Contains(current);
                current = current.BaseType)
            {
                var attrib = current.GetCustomAttribute<DeclaresEnumeratedValuesAttribute>();
                if (attrib == null)
                {
                    continue;
                }

                returned = true;
                yield return current;
            }

            // TODO: TBD: whether Type shouldn't just be returned always, no matter what.
            if (!returned)
            {
                yield return type;
            }
        }

        /// <summary>
        /// Returns whether <paramref name="value"/> Contains, so to speak, the
        /// <paramref name="mask"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="mask"></param>
        /// <returns></returns>
        /// <remarks>We cannot find any Bitwise Operators that might have been defined
        /// at the derived level from this angle, but we can see the member functions
        /// just fine.</remarks>
        /// <see cref="Keyed.Flags.Enumeration{T}.BitwiseAnd"/>
        /// <see cref="Keyed.Enumeration{TKey}.Equals(TKey)"/>
        public static bool Contains<T>(this T value, params T[] mask)
            where T : Keyed.Flags.Enumeration<T>
            => mask.Any(x => value.BitwiseAnd(value).Equals(x));
    }
}
