using System;

namespace Ellumination.Collections.Generic
{
    // ReSharper disable once IdentifierTypo
    internal static class Randomizer
    {
        private static int Seed => (int) (DateTime.Now.Ticks % int.MaxValue);

        public static Random Rnd { get; } = new Random(Seed);
    }
}
