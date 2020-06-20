//using System;
//using System.Collections.Generic;
//using System.Linq;

//namespace Ellumination.Collections
//{
//    using Xunit;

//    public class SerializedMaskValuesAttribute : CombinatorialValuesAttribute
//    {
//        private static IEnumerable<object> GetValues()
//        {
//            yield return (uint) 0x12121212;
//            yield return (uint) 0x34343434;
//            yield return (uint) 0x12341234;
//            yield return (uint) 0x56565656;
//            yield return (uint) 0x78787878;
//            yield return (uint) 0x56785678;
//            yield return (uint) 0x12345678;
//            yield return (uint) 0x15263748;
//        }

//        private static Lazy<object[]> LazyValues { get; }
//            = new Lazy<object[]>(() => GetValues().ToArray());

//        public SerializedMaskValuesAttribute()
//            : base(LazyValues.Value)
//        {
//        }
//    }
//}
