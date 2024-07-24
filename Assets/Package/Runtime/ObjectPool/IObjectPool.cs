namespace SnakeCore.ObjectPool
{
    
    /// <summary>
    /// Common interface that includes methods for returning object to pool.
    /// </summary>
    public interface IObjectPool
    {
        /// <summary>
        /// Returns object to the pool. If the object is not created by the pool, this function does nothing.
        /// </summary>
        /// <param name="obj">Object to be returned to pool.</param>
        public void ReturnObject(IPoolableObject obj);
        
        /// <summary>
        /// Returns all objects that are managed by the pool to the pool.
        /// </summary>
        public void ReturnAllObjects();

        /// <summary>
        /// Removes object from the pool and destroys it. If the object is not created by the pool, this function does nothing.
        /// </summary>
        /// <param name="obj">Object to be removed from pool.</param>
        public void Remove(IPoolableObject obj);
        
        /// <summary>
        /// Removes all objects from the pool.
        /// </summary>
        public void RemoveAll();
    }
    
    /// <summary>
    /// Common interface that includes methods for getting object from pool.
    /// </summary>
    public interface IObjectPoolGet
    {
        /// <summary>
        /// Gets object from the pool.
        /// </summary>
        /// <returns>Returns object from the pool.</returns>
        public IPoolableObject GetObject();
    }
    
    /// <summary>
    /// Common generic interface that includes methods for getting object from pool.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IObjectPoolGet<T> where T : IPoolableObject
    {
        /// <summary>
        /// Gets object from the pool.
        /// </summary>
        /// <returns>Returns object from the pool.</returns>
        public T GetObject();
    }
}
