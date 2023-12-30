// ==============================License==================================
// MIT License
// Author: Taha Mert Gökdemir
// =======================================================================
namespace TahaCore.Logging
{
    /// <summary>
    /// The interface for logging.
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Logs an error message.
        /// </summary>
        /// <param name="message">Message object to log. </param>
        void LogError(object message);
        
        /// <summary>
        /// Logs a warning message.
        /// </summary>
        /// <param name="message">Message object to log.</param>
        void LogWarning(object message);
        
        /// <summary>
        /// Logs an info message.
        /// </summary>
        /// <param name="message">Message object to log.</param>
        void LogInfo(object message);
    }
}