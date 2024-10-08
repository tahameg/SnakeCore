﻿// ==============================License==================================
// MIT License
// Author: Taha Mert Gökdemir
// =======================================================================

using VContainer;

namespace SnakeCore.DI
{
    /// <summary>
    /// include helper methods for converting LifetimeType to Lifetime
    /// </summary>
    internal static class LifetimeExtensions
    {
        internal static Lifetime ToLifetime(this LifetimeType lifetimeType)
        {
            if (lifetimeType == LifetimeType.Singleton) return Lifetime.Singleton;
            if (lifetimeType == LifetimeType.Instanced) return Lifetime.Transient;
            return Lifetime.Singleton;
        }
    }
}