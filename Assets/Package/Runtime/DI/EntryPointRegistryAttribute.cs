namespace SnakeCore.DI
{
    /// <summary>
    /// If defined with one of the other dependency injection attributes, the class will be registered as an entry point
    /// will automatically be created by the DI container. Entry points are lifecycle hooks that are called when the
    /// unity's lifecycle events are triggered. For example, <see cref="IStartable"/> will be called when the scope
    /// is first created.
    /// <see cref="https://vcontainer.hadashikick.jp/integrations/entrypoint">
    ///     VContainer documentation for all entry point interfaces.
    /// </see>
    /// <see cref="ApplicationRuntimeRegistryAttribute"/>
    /// <see cref="SceneRuntimeRegistryAttribute"/>
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Class)]
    public class EntryPointRegistryAttribute : System.Attribute
    {
    }
}