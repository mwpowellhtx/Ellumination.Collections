//using System;
//using System.Collections.Generic;
//using System.Linq;

//namespace Talks.Collections
//{
//    using Xunit;

//    public class RandomIntegerCombinatorialValuesAttribute : CombinatorialValuesAttribute
//    {
//        private static Random Rnd { get; } = new Random((int) DateTime.UtcNow.Ticks % int.MaxValue);

//        //private static IEnumerable<object> _values;

//        private static IEnumerable<object> PrivateValues
//        {
//            get
//            {
//                // ReSharper disable once InvertIf
//                IEnumerable<object> GetAll(int count)
//                {
//                    while (count-- > 0)
//                    {
//                        yield return Rnd.Next();
//                    }
//                }

//                //return _values ?? (_values = GetAll(3).ToArray());
//                return GetAll(3).ToArray();
//            }
//        }

//        public RandomIntegerCombinatorialValuesAttribute()
//            : base(PrivateValues.ToArray())
//        {
//        }
//    }
//}
