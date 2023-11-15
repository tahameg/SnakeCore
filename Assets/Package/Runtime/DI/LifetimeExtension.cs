using VContainer;

namespace TahaCore
{
    public static class LifetimeExtensions
    {
        public static Lifetime ToLifetime(this LifetimeType lifetimeType)
        {
            if (lifetimeType == LifetimeType.Singleton) return Lifetime.Singleton;
            if (lifetimeType == LifetimeType.Instanced) return Lifetime.Transient;
            return Lifetime.Singleton;
        }
    }
}