using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Ellumination.Collections
{
    using static BindingFlags;

    /// <summary>
    /// 
    /// </summary>
    public abstract partial class Enumeration
    {
        /// <summary>
        /// <see cref="Name"/> backing field.
        /// </summary>
        private string _name;

        /// <summary>
        /// Gets the Name of the Enumeration. This is either user furnished or derived by the declared field name.
        /// </summary>
        public virtual string Name
        {
            get => _name ?? (_name = ResolveName());
            protected internal set => _name = value;
        }

        /// <summary>
        /// Protected Constructor.
        /// </summary>
        /// <param name="name"></param>
        protected Enumeration(string name)
           : this(name, null)
        {
        }

        // TODO: TBD: inspect for Properties as well as Fields? i.e. especially for so-called 'lazy' initialized? barring the fact we pretty much want to initialize as soon as at least one are to be referenced, and initialize for ordinals, bitwise, etc
        /// <summary>
        /// Defines an appropriate set of PublicStaticDeclaredOnly.
        /// </summary>
        /// <see cref="Public"/>
        /// <see cref="Static"/>
        /// <see cref="DeclaredOnly"/>
        protected const BindingFlags PublicStaticDeclaredOnly = Public | Static | DeclaredOnly;

        /// <summary>
        /// Returns the declaring types.
        /// </summary>
        /// <param name="declaringTypes"></param>
        /// <returns></returns>
        protected static IEnumerable<Type> GetDeclaringTypes(params Type[] declaringTypes)
        {
            var types = declaringTypes
                .SelectMany(type => type.GetNestedTypes(PublicStaticDeclaredOnly))
                .Where(type => type.IsClass && type.IsStatic()).ToList();

            void PrependType(Type type) => types.Insert(0, type);

            declaringTypes.ToList().ForEach(PrependType);

            return types;
        }

        /// <summary>
        /// Resolves the Name based on the Enumeration instance declared in the Enumeration types.
        /// </summary>
        /// <returns></returns>
        protected abstract string ResolveName();

        /// <summary>
        /// Gets a transient <see cref="IDictionary{TKey,TValue}"/> keyed on the <see cref="Name"/>.
        /// </summary>
        protected static IDictionary<string, T> GetLookupsByName<T>()
            where T : Enumeration
            => GetValues<T>().ToDictionary(x => x.Name);

        /// <summary>
        /// Returns the First Or Default value from <see cref="GetValues{T}"/>
        /// for the given <paramref name="name"/>.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static T FromName<T>(string name)
            where T : Enumeration
            => GetLookupsByName<T>().TryGetValue(name, out var x) ? x : null;

        /// <summary>
        /// Returns the First Or Default value from <see cref="GetValues{T}"/>
        /// for the given <paramref name="name"/>.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="comparisonType"></param>
        /// <returns></returns>
        public static T FromName<T>(string name, StringComparison comparisonType)
            where T : Enumeration
        {
            try
            {
                return GetLookupsByName<T>().FirstOrDefault(x => string.Equals(x.Key, name, comparisonType)).Value;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Gets a transient <see cref="IDictionary{TKey,TValue}"/> keyed on the <see cref="DisplayName"/>.
        /// </summary>
        protected static IDictionary<string, T> GetLookupsByDisplayName<T>()
            where T : Enumeration
            => GetValues<T>().ToDictionary(x => x.DisplayName, x => x);

        /// <summary>
        /// Returns the First Or Default value from <see cref="GetValues{T}"/>
        /// for the given <paramref name="displayName"/>.
        /// </summary>
        /// <param name="displayName"></param>
        /// <returns></returns>
        public static T FromDisplayName<T>(string displayName)
            where T : Enumeration
            => GetLookupsByDisplayName<T>().TryGetValue(displayName, out var x) ? x : null;

        /// <summary>
        /// Returns the First Or Default value from <see cref="GetValues{T}"/>
        /// for the given <paramref name="displayName"/>.
        /// </summary>
        /// <param name="displayName"></param>
        /// <param name="comparisonType"></param>
        /// <returns></returns>
        public static T FromDisplayName<T>(string displayName, StringComparison comparisonType)
            where T : Enumeration
        {
            try
            {
                return GetLookupsByDisplayName<T>().FirstOrDefault(x => string.Equals(x.Key, displayName, comparisonType)).Value;
            }
            catch
            {
                return null;
            }
        }
    }
}
