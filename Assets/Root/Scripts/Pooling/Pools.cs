// Pools.cs

using System;

namespace Root.Scripts.Pooling
{
    public enum Pools
    {
        Null,
        StackElement1,
        StackElement2,
        StackElement3,
        StackElement4
    }

    public static class PoolsExtensions
    {
        public static string ToPoolName(this Pools pool) => pool.ToString();

        public static Pools NextPool(this Pools pool) =>
            pool switch
            {
                Pools.Null => Pools.StackElement1,
                Pools.StackElement1 => Pools.StackElement2,
                Pools.StackElement2 => Pools.StackElement3,
                Pools.StackElement3 => Pools.StackElement4,
                Pools.StackElement4 => Pools.StackElement1,
                _ => throw new ArgumentOutOfRangeException(nameof(pool), pool, null)
            };
    }
}