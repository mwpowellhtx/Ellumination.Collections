//using System;
//using System.Collections.Generic;
//using System.Linq;

//namespace Ellumination.Collections
//{
//    using Xunit;

//    public class RandomIntValuesAttribute : CombinatorialValuesAttribute
//    {
//        // TODO: TBD: not sure how much Lazy is hurting/helping here...
//        private static readonly Lazy<Random> LazyRnd = new Lazy<Random>(
//            () => new Random((int) DateTime.UtcNow.Ticks % int.MaxValue));

//        private static Random Rnd => LazyRnd.Value;

//        /// <summary>
//        /// Gets the Random Values for Internal use.
//        /// </summary>
//        internal static IEnumerable<uint> InternalRandomValues
//        {
//            get
//            {
//                IEnumerable<uint> GetAll(params uint[] values) => values;
//                return GetAll((uint) Rnd.Next(), (uint) Rnd.Next(), (uint) Rnd.Next()).ToArray();
//            }
//        }

//        private static Lazy<object[]> LazyValues { get; }
//            = new Lazy<object[]>(() => InternalRandomValues.Cast<object>().ToArray());

//        internal RandomIntValuesAttribute()
//            : base(LazyValues.Value)
//        {
//        }
//    }
//}
