
namespace SnakeCore.ObjectPool 
{
    /// <summary>
    /// Common interface that includes methods to manage life cycle of poolable objects.
    /// </summary>
    public interface IPoolableObject
    {
        /// <summary>
        /// Returns the object to the pool.
        /// </summary>
        void ReturnToPool();
    }
}
