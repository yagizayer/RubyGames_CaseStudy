// PoolAttribute.cs

using System;
using UnityEngine;

namespace Root.Scripts.Pooling
{
    [AttributeUsage(AttributeTargets.Field)]
    public class PoolAttribute : PropertyAttribute
    {
        public Pools Pool { get; }
        public int PoolSize { get; }
        public PoolAttribute(Pools pool, int poolSize = 1)
        {
            Pool = pool;
            PoolSize = poolSize;
        }
    }
}