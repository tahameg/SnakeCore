namespace SnakeCore.DI
{
    /// <summary>
    /// TahaCore Dependency Injector.
    /// Note: Instead use <see cref="ApplicationRuntimeRegistryAttribute"/> if possible.
    /// </summary>
    [ApplicationRuntimeRegistry(LifetimeType.Singleton, typeof(IInjector))]
    internal class DependencyInjector : IInjector
    {
        public void Inject(object obj)
        {
            SnakeCoreApplicationRuntime.Instance.Container.Inject(obj);
        }
    }
}