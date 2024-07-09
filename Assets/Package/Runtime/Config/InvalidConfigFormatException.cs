// ==============================License==================================
// MIT License
// Author: Taha Mert Gökdemir
// =======================================================================
namespace SnakeCore.Config
{
    /// <summary>
    /// Exception thrown when the config file is not in the correct format.
    /// </summary>
    public class InvalidConfigFormatException : System.Exception
    {
        /// <summary>
        /// Creates a new instance of InvalidConfigFormatException.
        /// </summary>
        /// <param name="message">Message.</param>
        /// <param name="line">Problematic line of the config data.</param>
        public InvalidConfigFormatException(string message, int line = 0) 
            : base($"{message} : at line {line}")
        {
        }
    }
}