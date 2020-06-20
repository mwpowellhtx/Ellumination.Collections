using System.Collections.Generic;
using System.Linq;

namespace Ellumination.Collections
{
    public abstract partial class Enumeration
    {
        // TODO: TBD: we may look at doing this from a shared utility method...
        // TODO: TBD: which would also serve the Enumeration<T> class ...
        /// <summary>
        /// Returns the Enumerated Values hosted by Type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ignoreNulls"></param>
        /// <returns></returns>
        internal static IEnumerable<T> GetValues<T>(bool ignoreNulls = true)
            where T : Enumeration
        {
            // Keep the formatting and implementation this way for troubleshooting purposes.
            var declaringTypes = GetDeclaringTypes(typeof(T)).Reverse().ToArray();

            // TODO: TBD: determine how to treat Null values... at the moment, it seems as though Null is being ignored...
            foreach (var values in declaringTypes
                .Select(type => type.GetFields(PublicStaticDeclaredOnly))
                .Select(fieldInfo => fieldInfo.Select(x => x.GetValue(null)).ToArray()))
            {
                foreach (var value in values.Where(x => !ignoreNulls || x != null))
                {
                    yield return (T) value;
                }

                //// TODO: TBD: this may work, but I'm not sure, if memory serves, there was a problem with this approach?
                //foreach (var value in values.OfType<TDerived>())
                //{
                //    yield return value;
                //}
            }
        }

        //// TODO: TBD: is nested class types a thing?
        //// TODO: TBD: and to what degree were we considering the use case?
        ///// <summary>
        ///// Gets the NestedClassTypes.
        ///// </summary>
        //private static IEnumerable<Type> NestedStaticClassTypes
        //    => typeof(TDerived).GetNestedTypes(PublicStaticDeclaredOnly)
        //        .Where(type => type.IsClass && type.IsStatic()).ToArray();
    }
}
