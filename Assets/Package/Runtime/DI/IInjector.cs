namespace SnakeCore.DI
{
    /// <summary>
    /// Interface for resolving implementations from the DI framework at runtime.
    /// </summary>
    public interface IInjector
    {
        public void Inject(object obj);
    }
}